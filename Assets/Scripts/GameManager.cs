using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager AskFor { get => instance; }
    private void Awake()
    {
        instance = this;
        maxPlayerHealth = playerHealth;
        mainCam = Camera.main;
        towerUpgradesUI.SetActive(false);
        RefreshCoinsUI();
        timeTextNumber.color = new Color(0.13f, 0.3f, 0.75f, 0.85f);
    }
    //-------------------------------
    private Camera mainCam;
    //-------------------------------

    [Header("Health")]
    [HideInInspector] public float maxPlayerHealth;
    public float playerHealth;
    public Healthbar healthbar;
    [Header("Enemies")]
    public List<Enemy> enemies;
    public List<Tower> towers;
    [Header("Towers and money")]
    public int coinsPerSecond = 1;
    public GameObject towerPrefab;
    public int coins;
    public TMPro.TMP_Text coinsText;
    public TMPro.TMP_Text passiveIncome_current;
    public TMPro.TMP_Text passiveIncome_Cost;
    private bool isPlacingTower = false;
    private GameObject selectedTower;
    public int passiveIncomeCost;
    public GameObject towerPlacementButtons;
    [Header("Tower Upgrades")]
    public GameObject towerUpgradesUI;
    [Header("Time Scale")]
    public TMPro.TMP_Text timeTextNumber;
    public TMPro.TMP_Text pauseText;
    [Header("Score System")]
    private uint scoreInt = 0;
    public GameObject gameOverUI;
    public TMPro.TMP_Text highscoreText;
    public TMPro.TMP_Text scoreText;
    public TMPro.TMP_Text ingameScoreText;
    [Header("Other Settings")]
    public GameObject restartGameUI;

    private void Start()
    {
        StartCoroutine(CoinsPerSecond());
        StartCoroutine(ScoreCounter());
        gameOverUI.SetActive(false);
        ingameScoreText.gameObject.SetActive(true);
        towerPlacementButtons.SetActive(false);
        HideRestartMenu();
    }
    private Vector3 towerDistanceDirection;
    private bool gotTowerDistDir = false;
    private void Update()
    {

        if (isPlacingTower)
        {
            if (SystemInfo.deviceType == DeviceType.Desktop)
            {
                selectedTower.transform.position = MousePos();
                if (Input.GetMouseButtonDown(0))
                {
                    ConfirmTowerPlacement();
                }
            }
            
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                if (Input.GetMouseButton(0))
                {
                    if (!gotTowerDistDir) towerDistanceDirection = (Vector3)MousePos() - selectedTower.transform.position;

                    gotTowerDistDir = true;
                    selectedTower.transform.position = (Vector3)MousePos() - towerDistanceDirection;
                }
                if (Input.GetMouseButtonUp(0))
                {
                    gotTowerDistDir = false;
                }
            }
        }

        SelectTower();
        if (Input.GetMouseButtonDown(1))
        {
            Deselect();
            if (isPlacingTower)
            {
                CancelTowerPlacement();
            }
        }
    }

    public void RefreshCoinsUI()
    {
        coinsText.text = coins.ToString();
    }
    private bool towerIsSelected = false;
    public void SelectTower()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (towerIsSelected) return;
            bool aMatchWasFound = false;
            for (int i = 0; i < towers.Count; i++)
            {
                Collider2D col = towers[i].GetComponent<Collider2D>();
                Tower t = towers[i].GetComponent<Tower>();
                for (int j = 0; j < MousePosColliders().Length; j++)
                {
                    if (MousePosColliders()[j] == col)
                    {
                        aMatchWasFound = true;
                        if (towerIsSelected) break;
                        t.rangeIndicator.SetActive(true);
                        if (!isPlacingTower)
                        {
                            ShowTowerUpgrades(t);
                            towerIsSelected = true;
                            SoundManager.AskFor.ClickSound();
                        }
                        break;
                    }
                }

            }
            if (!aMatchWasFound && !isPlacingTower)
            {
                Deselect();
            }
        }


    }

    private bool isMovingStatStore = false;
    public void ReleaseStatStore(BaseEventData data)
    {
        isMovingStatStore = false;
    }
    private Vector3 distanceDirection = Vector3.zero;
    public void MoveStateStoreAround(BaseEventData data)
    {
        if (!isMovingStatStore) distanceDirection = Input.mousePosition - towerUpgradesUI.transform.position;

        isMovingStatStore = true;
        towerUpgradesUI.transform.position = Input.mousePosition - distanceDirection;
    }

    public void ShowTowerUpgrades(Tower t)
    {
        towerUpgradesUI.GetComponent<TowerUpgradesUI>().OverwriteUIinfo(t);
        towerUpgradesUI.SetActive(true);

        RectTransform rt = towerUpgradesUI.GetComponent<RectTransform>();
        if (!RendererExtensions.IsFullyVisibleFrom(rt))
        {
            towerUpgradesUI.transform.position = Input.mousePosition;
        }
    }

    public void Deselect()
    {
        foreach (Tower t in towers)
        {
            t.rangeIndicator.SetActive(false);
        }
        towerUpgradesUI.SetActive(false);
        towerIsSelected = false;
    }
    private Collider2D[] MousePosColliders()
    {
        Collider2D[] colliders;
        colliders = Physics2D.OverlapCircleAll(MousePos(), 0.0f);
        return colliders;
    }
    public Vector2 MousePos()
    {
        Vector3 worldPosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
        return worldPosition;
    }

    public void BuyAndPlaceTower()
    {

        int price = towerPrefab.GetComponent<Tower>().price;
        if (price <= coins)
        {
            coins -= price;
            selectedTower = Instantiate(towerPrefab, Vector2.zero, transform.rotation);
            isPlacingTower = true;
            RefreshCoinsUI();
            SoundManager.AskFor.ClickSound();
        }

        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            towerPlacementButtons.SetActive(true);
        }
    }
    public void ConfirmTowerPlacement()
    {
        Tower t = selectedTower.GetComponent<Tower>();
        t.rangeIndicator.SetActive(false);
        t.canShoot = true;
        isPlacingTower = false;
        selectedTower = null;

        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            towerPlacementButtons.SetActive(false);
        }
    }
    public void CancelTowerPlacement()
    {
        Tower t = selectedTower.GetComponent<Tower>();
        coins += t.price;
        RefreshCoinsUI();
        GameManager.AskFor.towers.Remove(t);
        Destroy(selectedTower);

        isPlacingTower = false;
        selectedTower = null;

        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            towerPlacementButtons.SetActive(false);
        }
    }
    public void PlayerTakesDamage(float damage)
    {
        playerHealth -= damage;
        healthbar.RefreshHealthbar();
        if (playerHealth <= 0)
        {
            GameOver();
            SoundManager.AskFor.PlayerDamage();
        }
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        RefreshCoinsUI();
    }

    public IEnumerator CoinsPerSecond()
    {
        while(true)
        {
            yield return new WaitForSeconds(1f);
            AddCoins(coinsPerSecond);
        }
    }
    public IEnumerator ScoreCounter()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.8f);
            AddToScore(1);
        }
    }
    public void AddToScore(uint amount)
    {
        scoreInt += amount;
        ingameScoreText.text = scoreInt.ToString();
    }

    public void UpgradePassiveIncome()
    {
        if (coins < passiveIncomeCost) return;
        
        
        coins -= passiveIncomeCost;
        coinsPerSecond++;
        passiveIncomeCost *= 2;

        passiveIncome_current.text = coinsPerSecond.ToString();
        passiveIncome_Cost.text = passiveIncomeCost.ToString();
        RefreshCoinsUI();
        SoundManager.AskFor.ClickSound();
    }

    public void GameOver()
    {
        ingameScoreText.gameObject.SetActive(false);
        savedTimeScale = Time.timeScale;
        Time.timeScale = 0;
        gameOverUI.SetActive(true);

        if (scoreInt >= Convert.ToUInt32(PlayerPrefs.GetString("highscore", "0")))
        {
            PlayerPrefs.SetString("highscore", scoreInt.ToString());
        }

        scoreText.text = scoreInt.ToString();
        highscoreText.text = PlayerPrefs.GetString("highscore", "0");
    }
    private float savedTimeScale;
    public void PlayAgain()
    {
        SoundManager.AskFor.ClickSound();
        Scene scene = SceneManager.GetActiveScene(); SceneManager.LoadScene(scene.name, LoadSceneMode.Single);
        Time.timeScale = 1f;
    }
    public void ShowRestartMenu()
    {
        restartGameUI.SetActive(true);
    }
    public void HideRestartMenu()
    {
        restartGameUI.SetActive(false);
    }
    public void TogglePauseUnpause()
    {
        SoundManager.AskFor.ClickSound();
        if (Time.timeScale == 0)
        {
            Time.timeScale = savedTimeScale;
            pauseText.text = "II";
        }
        else 
        {
            savedTimeScale = Time.timeScale;
            Time.timeScale = 0;
            pauseText.text = "T";
        }
    }
    public void ToggleTimeScale123()
    {
        SoundManager.AskFor.ClickSound();
        if (Time.timeScale == 0) return;
        Color color = new Color(1, 1, 1, 1);
        switch (Time.timeScale)
        {
            case 1:
                {
                    color = new Color(0.3f, 0.13f, 0.75f, 0.85f);
                    Time.timeScale = 3;
                    break;
                }
            case 3:
                {
                    color = new Color(0.5f, 0.13f, 0.1f, 0.85f);
                    Time.timeScale = 5;
                    break;
                }
            case 5:
                {
                    color = new Color(0.13f, 0.3f, 0.75f, 0.85f);
                    Time.timeScale = 1;
                    break;
                }
        }
        timeTextNumber.text = Time.timeScale.ToString();
        timeTextNumber.color = color;
    }

}
