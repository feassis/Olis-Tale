using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameoverScreenGraphics : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recordValue;
    [SerializeField] private TextMeshProUGUI currentValue;
    [SerializeField] private Button restart;

    private void Awake()
    {
        restart.onClick.AddListener(OnRestartButtonClicked);
    }

    public void Open()
    {
        gameObject.SetActive(true);

        recordValue.text = PlayerPrefs.GetInt("Wallet", 0).ToString();
        currentValue.text = PlayerManager.instance.PlayerWallet.ToString();

        if (PlayerManager.instance.PlayerWallet > PlayerPrefs.GetInt("Wallet", 0))
        {
            PlayerPrefs.SetInt("Wallet", PlayerManager.instance.PlayerWallet);
        }
        Time.timeScale = 0;
    }

    private void OnRestartButtonClicked()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("SampleScene");
    }
}
