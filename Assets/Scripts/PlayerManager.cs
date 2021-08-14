using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerManager : MonoBehaviour
{
    #region Singleton

    public static PlayerManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

    private int playerWallet;
    [HideInInspector]public int PlayerWallet
    {
        get => playerWallet;
        set
        {
            playerWallet = value;
            walletText.text = playerWallet.ToString();
        }
    }

    public GameObject Player;
    public GameObject Inventory;
    public GameObject Equipment;
    public Slider HealthBar;
    public GameoverScreenGraphics GameOver;

    [SerializeField] private TextMeshProUGUI walletText;

    public void EarnMoney(int amount)
    {
        PlayerWallet += amount;
    }

    public void LoseMoney(int amount)
    {
        PlayerWallet -= amount;
        if(PlayerWallet < 0)
        {
            Debug.Log("Game Over");
        }
    }
}
