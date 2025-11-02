using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleSystem : MonoBehaviour
{

    enum State
    {
        Start,
        ActionSelection,
        MoveSelection,
        RunTurns,
        BattleOver,
    }

    State state;

    public UnityAction OnBattleOver;
    [SerializeField] ActionSelectionUI actionSelectionUI;
    [SerializeField] MoveSelection moveSelection;
    [SerializeField] BattleDialog battleDialog;
    [SerializeField] BattleUnit playerUnit;
    [SerializeField] BattleUnit enemyUnit;


    public void BattleStart(Battler player, Battler enemy)
    {
        state = State.Start;
        Debug.Log("BattleStart!");
        actionSelectionUI.Init();
        moveSelection.Init();
        actionSelectionUI.Close();
        StartCoroutine(SetUpBattle(player,enemy));
    }

    IEnumerator SetUpBattle(Battler player,Battler enemy)
    {
        playerUnit.Setup(player);
        enemyUnit.Setup(enemy);
        yield return battleDialog.TypeDialog($"{enemy.Base.Name}があらわれた!\nどうする？");
        ActionSelection();
    }

    void BattleOver()
    {
        OnBattleOver?.Invoke();
    }

    void ActionSelection()
    {
        state = State.ActionSelection;
        actionSelectionUI.Open();
    }
    void MoveSelection()
    {
        state = State.MoveSelection;
        moveSelection.Open();
    }
    IEnumerator RunTurns()
    {
        state = State.RunTurns;

        yield return RunMove(playerUnit, enemyUnit);
        if(state == State.BattleOver)
        {
            yield return battleDialog.TypeDialog($"{enemyUnit.Battler.Base.Name}を倒した！");
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
            BattleOver();

            yield break;
        }
        yield return RunMove(enemyUnit, playerUnit);
        if (state == State.BattleOver)
        {
            yield return battleDialog.TypeDialog($"{playerUnit.Battler.Base.Name}は力尽きた");
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));

            BattleOver();
            yield break;
        }


        yield return battleDialog.TypeDialog("どうする？");
        ActionSelection();
    }

    IEnumerator RunMove(BattleUnit sourceUnit, BattleUnit targetUnit)
    {
        int damage = targetUnit.Battler.TakeDamage(sourceUnit.Battler);
        yield return battleDialog.TypeDialog($"{sourceUnit.Battler.Base.Name}の攻撃\n{targetUnit.Battler.Base.Name}は{damage}のダメージ", auto: false);
        targetUnit.UpdateUI();

        if(targetUnit.Battler.HP <= 0)
        {
            state = State.BattleOver;
        }
    }


    private void Update()
    {
        switch (state)
        {
            case State.Start:
                break;
            case State.ActionSelection:
                HandleActionSelection();
                break;
            case State.MoveSelection:
                HandleMoveSelection();
                break;
            case State.BattleOver:
                break;
        }
    }
    void HandleActionSelection()
    {
        actionSelectionUI.HandleUpdate();

        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(actionSelectionUI.SelectedIndex == 0)
            {
                MoveSelection();
            }
            else if (actionSelectionUI.SelectedIndex == 1)
            {
                Debug.Log("にげる");
                BattleOver();
            }
        }
    }
    void HandleMoveSelection()
    {
        moveSelection.HandleUpdate();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            actionSelectionUI.Close();
            moveSelection.Close();
            StartCoroutine(RunTurns());
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            moveSelection.Close();
            ActionSelection();

        }

    }


}
