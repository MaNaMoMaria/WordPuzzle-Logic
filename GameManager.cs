using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

/**
 * 📝 GameManager: Handles core word puzzle logic, UI updates, and game economy.
 */


namespace WordPuzzle.Prototype
{
    public class GameManager : MonoBehaviour
    {
        [Header("🖼️ UI Elements")]
        public TextMeshProUGUI wordDisplayText;
        public TextMeshProUGUI timerText; // Optional: Leave empty in inspector to hide
        public TextMeshProUGUI levelText;
        public TextMeshProUGUI coinText;
        public GameObject nextLevelPanel;
        public TextMeshProUGUI nextLevelButtonText;

        [Header("🔘 Buttons Setup")]
        public List<Button> gameButtons;

        [Header("⚙️ Game Settings")]
        public List<string> wordList = new List<string> { "UNITY", "GAME", "CODE", "LEVEL" };
        public Sprite wrongIconSprite;

        [Header("💰 Economy Settings")]
        public int coins = 0;
        public int prizePerLevel = 10;
        public int hintCost = 30;

        private int currentWordIndex = 0;
        private string targetWord;
        private char [] displayArray;
        private float currentTime; // Tracks elapsed time per level
        private bool isGameOver = false;
        private int correctCount = 0;

        void Start ()
        {
            LoadData();
            LoadLevel();
        }

        // 🔄 Initializes the current level state
        void LoadLevel ()
        {
            if (currentWordIndex < wordList.Count)
            {
                targetWord = wordList [currentWordIndex].ToUpper();
                currentTime = 0; // Reset timer for each level
                correctCount = 0;
                isGameOver = false;

                if (nextLevelPanel != null) nextLevelPanel.SetActive(false);
                if (levelText != null) levelText.text = "Level: " + (currentWordIndex + 1);
                UpdateUI();

                SetupButtonsForWord();

                displayArray = new char [targetWord.Length];
                for (int i = 0; i < targetWord.Length; i++) displayArray [i] = '_';
                UpdateDisplay();
            }
            else
            {
                wordDisplayText.text = "ALL DONE!";
                isGameOver = true;
            }
        }

        // ⌨️ Prepares letter buttons with characters and extra decoys
        void SetupButtonsForWord ()
        {
            // 1. Calculate required buttons (Word length + 3 random decoys)
            int requiredButtonsCount = targetWord.Length + 3;
            requiredButtonsCount = Mathf.Min(requiredButtonsCount, gameButtons.Count);

            List<char> lettersForButtons = new List<char>();

            // Add all letters from the target word (including duplicates)
            foreach (char c in targetWord)
            {
                lettersForButtons.Add(c);
            }

            // 2. Add decoy letters (characters not present in the target word)
            string allAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            while (lettersForButtons.Count < requiredButtonsCount)
            {
                char randomChar = allAlphabet [Random.Range(0, allAlphabet.Length)];
                if (!targetWord.Contains(randomChar.ToString()))
                {
                    lettersForButtons.Add(randomChar);
                }
            }

            // 3. Shuffle the letters list
            for (int i = 0; i < lettersForButtons.Count; i++)
            {
                char temp = lettersForButtons [i];
                int randomIndex = Random.Range(i, lettersForButtons.Count);
                lettersForButtons [i] = lettersForButtons [randomIndex];
                lettersForButtons [randomIndex] = temp;
            }

            // 4. Activate and configure UI buttons
            for (int i = 0; i < gameButtons.Count; i++)
            {
                if (i < requiredButtonsCount)
                {
                    gameButtons [i].gameObject.SetActive(true);
                    gameButtons [i].interactable = true;
                    TextMeshProUGUI btnText = gameButtons [i].GetComponentInChildren<TextMeshProUGUI>();
                    if (btnText != null)
                    {
                        btnText.text = lettersForButtons [i].ToString();
                        string currentLetter = btnText.text;
                        gameButtons [i].onClick.RemoveAllListeners();
                        gameButtons [i].onClick.AddListener(() => AddLetter(currentLetter));
                    }
                }
                else
                {
                    gameButtons [i].gameObject.SetActive(false);
                }
            }
        }

        // 🎯 Triggered when a letter button is clicked
        public void AddLetter (string letter)
        {
            if (isGameOver) return;
            char inputChar = letter.ToUpper() [0];
            bool foundAny = false;
            GameObject clickedButton = UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject;

            // Find and fill the first empty slot for this letter
            for (int i = 0; i < targetWord.Length; i++)
            {
                if (targetWord [i] == inputChar && displayArray [i] == '_')
                {
                    displayArray [i] = inputChar;
                    correctCount++;
                    foundAny = true;
                    break; // Exit after finding the first match
                }
            }

            if (foundAny)
            {
                UpdateDisplay();
                if (clickedButton != null) clickedButton.GetComponent<Button>().interactable = false;

                if (correctCount == targetWord.Length)
                {
                    isGameOver = true;
                    coins += prizePerLevel;
                    SaveData();
                    ShowNextLevelPanel();
                }
            }
            else
            {
                if (clickedButton != null && wrongIconSprite != null) ShowWrongFeedback(clickedButton);
            }
        }

        // 💡 Spends coins to reveal one hidden letter
        public void UseHint ()
        {
            if (coins >= hintCost && !isGameOver)
            {
                for (int i = 0; i < targetWord.Length; i++)
                {
                    if (displayArray [i] == '_')
                    {
                        char letterToReveal = targetWord [i];
                        displayArray [i] = letterToReveal;
                        coins -= hintCost;
                        correctCount++;
                        UpdateDisplay();
                        UpdateUI();
                        SaveData();
                        if (correctCount == targetWord.Length)
                        {
                            isGameOver = true;
                            ShowNextLevelPanel();
                        }
                        break;
                    }
                }
            }
        }

        // ✨ Displays the win panel
        void ShowNextLevelPanel ()
        {
            if (nextLevelPanel != null)
            {
                if (nextLevelButtonText != null)
                    nextLevelButtonText.text = "LEVEL " + (currentWordIndex + 2);
                nextLevelPanel.SetActive(true);
            }
        }

        // ⏭️ Proceeds to the next word in the list
        public void GoToNextLevel ()
        {
            currentWordIndex++;
            SaveData();
            LoadLevel();
        }

        void Update ()
        {
            if (isGameOver) return;

            // Increment time (stopwatch style)
            currentTime += Time.deltaTime;
            if (timerText != null)
            {
                timerText.text = "Time: " + Mathf.Floor(currentTime);
            }
        }

        // 💾 Persists progress to local storage
        void SaveData ()
        {
            PlayerPrefs.SetInt("UserCoins", coins);
            PlayerPrefs.SetInt("UserLevel", currentWordIndex);
            PlayerPrefs.Save();
            UpdateUI();
        }

        // 📂 Loads progress from local storage
        void LoadData ()
        {
            coins = PlayerPrefs.GetInt("UserCoins", 0);
            currentWordIndex = PlayerPrefs.GetInt("UserLevel", 0);
        }

        // 💎 Refreshes the currency UI
        void UpdateUI ()
        {
            if (coinText != null) coinText.text = "Coins: " + coins;
        }

        // 🧹 Resets all saved progress and reloads scene
        public void ClearAllData ()
        {
            PlayerPrefs.DeleteAll();
            coins = 0;
            currentWordIndex = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        // ❌ Spawns a temporary "wrong" icon on the clicked button
        void ShowWrongFeedback (GameObject btn)
        {
            GameObject iconObj = new GameObject("WrongIcon");
            iconObj.transform.SetParent(btn.transform);
            iconObj.transform.localPosition = Vector3.zero;
            iconObj.transform.localScale = Vector3.one;
            Image img = iconObj.AddComponent<Image>();
            img.sprite = wrongIconSprite;
            img.raycastTarget = false;
            Destroy(iconObj, 0.5f);
        }

        [Header("🛠️ Settings Panel")]
        public GameObject settingsPanel;

        // 🔓 Opens the settings menu
        public void OpenSettings ()
        {
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(true);
                // Optional: Time.timeScale = 0; to pause gameplay
            }
        }

        // 🔒 Closes the settings menu
        public void CloseSettings ()
        {
            if (settingsPanel != null)
            {
                settingsPanel.SetActive(false);
                // Optional: Time.timeScale = 1; to resume gameplay
            }
        }

        // 🔡 Refreshes the word visual display (e.g., U N I _ Y)
        void UpdateDisplay () { wordDisplayText.text = string.Join(" ", displayArray); }
    }
}