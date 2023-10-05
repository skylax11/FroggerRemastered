using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Handler : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _winText;
    [SerializeField] TextMeshProUGUI _lostText;
    [SerializeField] TextMeshProUGUI _score;
    [SerializeField] TextMeshProUGUI _timeBonusText;

    public void LostText(float score, float timeBonus)
    {
        _lostText.text = "You have lost! Your new score : " + "  " + score;
        _timeBonusText.text = "Time Deduction:" + " " + timeBonus;
    }
    public void WinText(float score, float timeBonus)
    {
        _winText.text = "You have found the treasure! Your new score : " + "  " + score;
        _timeBonusText.text = "Time Deduction:" + " " + timeBonus;
    }
    public void ReturnMenu()
    {
        MenuManager.Instance.gameObject.SetActive(false);
        SceneManager.LoadScene(0);
    }
    public void UpdateScore(float score)
    {
        _score.text = "Score:" + " " + score;
    }
}
