using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommandInput : MonoBehaviour
{
    public float inputBufferTime = 0.5f;
    public Player player;

    private struct InputEntry
    {
        public KeyCode key;
        public float time;
        public InputEntry(KeyCode key, float time)
        {
            this.key = key;
            this.time = time;
        }
    }

    private Queue<InputEntry> inputBuffer = new Queue<InputEntry>();

    // 커맨드 이름 -> (입력 시퀀스, 유효 상태 타입들) 리스트
    private Dictionary<string, List<(List<KeyCode> sequence, List<Type> validStates)>> commandMap = new();

    void Start()
    {
        player = GetComponent<Player>();
        // 예시 커맨드 등록
        RegisterCommand("DashAttack", new List<KeyCode> { KeyCode.A, KeyCode.A, KeyCode.Mouse0 }, new List<Type> { typeof(PlayerGroundState), typeof(PlayerAirState), typeof(PlayerJumpState) });
        RegisterCommand("DashAttack", new List<KeyCode> { KeyCode.D, KeyCode.D, KeyCode.Mouse0 }, new List<Type> { typeof(PlayerGroundState), typeof(PlayerAirState), typeof(PlayerJumpState) });

        RegisterCommand("DownAttack", new List<KeyCode> { KeyCode.S, KeyCode.S, KeyCode.Mouse0 }, new List<Type> { typeof(PlayerAirState), typeof(PlayerJumpState) });
        RegisterCommand("Attack", new List<KeyCode> { KeyCode.Mouse0 });
    }

    void Update()
    {
        CheckInput();
        CleanBuffer();
        CheckCommands();
    }

    void CheckInput()
    {
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(key))
                inputBuffer.Enqueue(new InputEntry(key, Time.time));
        }
    }

    void CleanBuffer()
    {
        while (inputBuffer.Count > 0 && Time.time - inputBuffer.Peek().time > inputBufferTime)
        {
            inputBuffer.Dequeue();
        }
    }

    void CheckCommands()
    {
        List<KeyCode> currentInputs = inputBuffer.Select(e => e.key).ToList();
        var currentState = player.stateMachine.state;

        foreach (var pair in commandMap)
        {
            foreach (var (sequence, validStates) in pair.Value)
            {
                if (IsMatch(currentInputs, sequence) && IsValidState(currentState, validStates))
                {
                    Debug.Log($"{pair.Key} 커맨드 발동");
                    inputBuffer.Clear();
                    ExecuteCommand(pair.Key);
                    return;
                }
            }
        }
    }
    bool IsValidState(PlayerState currentState, List<Type> validStates)
    {
        if (validStates == null || validStates.Count == 0)
            return true; // 조건이 없으면 모든 상태에서 허용
        Debug.Log($"현재 상태: {currentState.GetType()}");
        foreach (var validType in validStates)
        {
            Debug.Log($"검사 대상 타입: {validType}");
            Debug.Log($"결과: {validType.IsAssignableFrom(currentState.GetType())}");
            if (validType != null && currentState != null && validType.IsInstanceOfType(currentState))
                return true;
        }
        return false;
    }

    void ExecuteCommand(string commandName)
    {
        try
        {
            var ability = player.curAbility;
            var abilityType = ability.GetType();

            string methodName = commandName switch
            {
                "DashAttack" => "DashAttackHandle",
                "DownAttack" => "DownAttackHandle",
                "Attack" => "AttackHandle",
                _ => "AttackHandle"
            };

            var method = abilityType.GetMethod(methodName,
                System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            // 부모(Player_Ability)에만 정의되어 있고 자식에서 오버라이드 안 한 경우
            bool isOverridden = method != null && method.DeclaringType == abilityType;

            if (method != null && isOverridden)
            {
                method.Invoke(ability, null);
            }
            else
            {
                Debug.LogWarning($"{methodName}이 {abilityType.Name}에 오버라이드되어 있지 않음. 기본 공격 실행");
                ability.AttackHandle();
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"ExecuteCommand 오류: {ex.Message}");
        }
    }
    bool IsMatch(List<KeyCode> buffer, List<KeyCode> command)
    {
        if (buffer.Count < command.Count) return false;

        int start = buffer.Count - command.Count;
        for (int i = 0; i < command.Count; i++)
        {
            if (buffer[start + i] != command[i])
                return false;
        }
        return true;
    }

    public void RegisterCommand(string name, List<KeyCode> inputSequence, List<Type> validStates)
    {
        if (!commandMap.ContainsKey(name))
        {
            commandMap[name] = new List<(List<KeyCode>, List<Type>)>();
        }
        commandMap[name].Add((inputSequence, validStates));
    }

    // 상태 체크 없이 등록하고 싶을 경우를 위한 오버로드
    public void RegisterCommand(string name, List<KeyCode> inputSequence)
    {
        RegisterCommand(name, inputSequence, new List<Type>()); // 빈 타입 리스트 → 모든 상태에서 허용
    }
}