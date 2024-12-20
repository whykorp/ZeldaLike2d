using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.UI;

public class InFightPlayerAction : MonoBehaviour
{
    public FightManager fightManager;
    public HealthBar enemyHealthBar;
    public HealthBar playerHealthBar;
    public GameObject inputField;
    public Text inputFieldResult;
    public Text inputFieldPlaceholder;
    public string inputFieldContent;
    public float typingTimer;
    public float typingDuration;
    bool isEnterPressed;
    bool isGoodLineEnter;
    public float critDificulty=5;

    void Awake()
    {
        inputField.SetActive(false);
    }

    void Update()
    {
        typingTimer+=0.01666666666f;
        isEnterPressed=Input.GetKeyDown(KeyCode.Return);
        
    }
    public IEnumerator GetPlayerCommandLine(string commandLineToType)
    {
        Debug.Log("démarééé");
        Debug.Log(commandLineToType);
        inputField.SetActive(true);
        inputFieldPlaceholder.text=commandLineToType;
        
        typingTimer=0;
        yield return new WaitUntil(()=>isEnterPressed==true);
        typingDuration=typingTimer;
        if(inputFieldResult.text==commandLineToType)
        {
            isGoodLineEnter=true;
        }
        else
        {
            isGoodLineEnter=false;
        }
        inputField.GetComponent<InputField>().text="";
        inputField.SetActive(false);
    }

    void CreateBuff(string _buffType, string _buffName, BuffManager.Buff _buff)
    {
        FightManager.activeBuffs.Add(_buffName, _buff);
        if(_buffType=="attack")
        {
            PlayerStats.playerCurrentAttack *= _buff.value;  // Applique le buff immédiatement
        }
        else if(_buffType=="defense")
        {
            PlayerStats.playerCurrentDefense *= _buff.value;  // Applique le buff immédiatement
        }

        Debug.Log("Buff "+ _buff.type+ " appliqué ! "+_buff.value+" % "+_buffType+" pour " +(_buff.duration-1)+ " tours.");
    }


    //SQL:
    //Module 1:
    //PlayerStats.playerCurrentAttack,InFightMainMenu.enemyCurrentDefense
    public IEnumerator SimpleAttack()
    {
        Debug.Log("coroutine started");
        StartCoroutine(GetPlayerCommandLine("simple_attack()"));
        yield return new WaitUntil(()=>isEnterPressed);
        if(isGoodLineEnter)
        {
            if(typingDuration<critDificulty)
            {
                FightManager.enemyCurrentHealth-=45*PlayerStats.playerCurrentAttack*PlayerStats.playerAttackCoeficien/FightManager.enemyCurrentDefense;
                FightManager.playerAction="Kriss utilise Simple_Attack(), coup critique!";
            }
            else
            {
                FightManager.enemyCurrentHealth-=30*PlayerStats.playerCurrentAttack*PlayerStats.playerAttackCoeficien/FightManager.enemyCurrentDefense;
                FightManager.playerAction="Kriss utilise Simple_Attack()";
            }
            //Debug.Log(FightManager.enemyCurrentHealth);
            enemyHealthBar.SetHealth(FightManager.enemyCurrentHealth);
            
            FightManager.isPlayerTurn=false;
        }
        else
        {
            FightManager.playerAction="Kriss a échoué simple Attack()";
            FightManager.isPlayerTurn=false;
        }
        
    }

    public IEnumerator PowerAttack()
    {
        StartCoroutine(GetPlayerCommandLine("power_attack()"));
        yield return new WaitUntil(()=>isEnterPressed);
        if(isGoodLineEnter)
        {
            float rand = UnityEngine.Random.Range(0f, 1f);
            if(rand > 0.49f)  // 50% chance to succeed
            {
                if(typingDuration<critDificulty)
                {
                    FightManager.enemyCurrentHealth -= 100 * PlayerStats.playerCurrentAttack * PlayerStats.playerAttackCoeficien / FightManager.enemyCurrentDefense;
                    FightManager.playerAction = "Kriss utilise Power_Attack(), coup critique!";
                }
                else
                {
                    FightManager.enemyCurrentHealth -= 75 * PlayerStats.playerCurrentAttack * PlayerStats.playerAttackCoeficien / FightManager.enemyCurrentDefense;
                    FightManager.playerAction = "Kriss utilise Power_Attack()";
                }
                
                Debug.Log("Power_Attack successful! Enemy Health: " + FightManager.enemyCurrentHealth);
                enemyHealthBar.SetHealth(FightManager.enemyCurrentHealth);
                
            }
            else
            {
                Debug.Log("Power_Attack failed!");
                FightManager.playerAction = "Kriss a échoué Power_Attack()";
            }
            FightManager.isPlayerTurn = false;
        }
        else
        {
            FightManager.playerAction="Kriss a échoué Power_Attack()";
            FightManager.isPlayerTurn=false;
        }
        
    }

    // Healing Query to restore player health
    public IEnumerator HealingQuery()
    {
        StartCoroutine(GetPlayerCommandLine("healing_query()"));
        yield return new WaitUntil(()=>isEnterPressed);
        if(isGoodLineEnter)
        {
            if(typingDuration<critDificulty)
            {
                float healAmount = 0.40f * PlayerStats.playerMaxHealth;
                PlayerStats.playerCurrentHealth = Mathf.Min(PlayerStats.playerMaxHealth,PlayerStats.playerCurrentHealth + healAmount);
                FightManager.playerAction = "Kriss utilise Healing_Query(), coup critique!";
            }
            else
            {
                float healAmount = 0.25f * PlayerStats.playerMaxHealth;
                PlayerStats.playerCurrentHealth = Mathf.Min(PlayerStats.playerMaxHealth,PlayerStats.playerCurrentHealth + healAmount);
                FightManager.playerAction = "Kriss utilise Healing_Query()";
            }
            
            playerHealthBar.SetHealth(PlayerStats.playerCurrentHealth);
            FightManager.isPlayerTurn = false;
        }
        else
        {
            FightManager.playerAction="Kriss a échoué Healing_Query()";
            FightManager.isPlayerTurn=false;
        }
        
    }

    public IEnumerator SynergisticBuff()
    {
        StartCoroutine(GetPlayerCommandLine("synergistic_buff()"));
        yield return new WaitUntil(()=>isEnterPressed);
        if(isGoodLineEnter)
        {
            if(typingDuration<critDificulty)
            {
                BuffManager.Buff SynergisticBuffCrit = new BuffManager.Buff("SynergisticBuff", 1.7f, 3+1);  // +50% attaque pour 3 tours
                CreateBuff("attack","SynergisticBuff", SynergisticBuffCrit);
                FightManager.playerAction = "Kriss utilise Synergistic Buff, coup critique!";
            }
            else
            {
                BuffManager.Buff SynergisticBuff = new BuffManager.Buff("SynergisticBuff", 1.5f, 3+1);  // +50% attaque pour 3 tours
                CreateBuff("attack","SynergisticBuff", SynergisticBuff);
                FightManager.playerAction = "Kriss utilise Synergistic Buff";
            }
            FightManager.isPlayerTurn = false;
        }
        else
        {
            FightManager.playerAction="Kriss a échoué Synergistic_Buff()";
            FightManager.isPlayerTurn=false;
        }
        
    }

    public void FirewallUpgrade()
    {
        BuffManager.Buff FirewallUpgrade = new BuffManager.Buff("FirewallUpgrade", 1.5f, 3+1);  // +50% de défense pour 3 tours
        CreateBuff("defense","FirewallUpgrade", FirewallUpgrade);
        FightManager.playerAction = "Kriss utilise Firewall Upgrade";
        FightManager.isPlayerTurn = false;
    }


    //SQL:
    //Module 2:
    
    
}
