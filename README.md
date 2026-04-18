# WordPuzzle-Logic
A professional Unity-based Word Puzzle system with dynamic level generation, persistent data (PlayerPrefs), and hint/coin economy logic.


# WordPuzzle_prototype-
A professional Unity-based Word Puzzle system with dynamic level generation, persistent data (PlayerPrefs), and hint/coin economy logic.

# 🧩 Word Puzzle Pro - Unity Core System

A professional, high-performance word puzzle logic system built with **Unity** and **C#**. This project demonstrates a complete game loop including dynamic level generation, game economy, and persistent data management.

---

## 🚀 Key Features

* **Dynamic Word Generation**: Automatically shuffles letters and injects decoy characters to increase difficulty.
* **Game Economy (Coin System)**: Players earn coins by solving puzzles and can spend them on hints.
* **Intelligent Hint System**: A non-intrusive hint logic that reveals characters one by one.
* **Persistent Progress**: Uses `PlayerPrefs` to save levels, coins, and settings even after closing the app.
* **Responsive UI**: Optimized for different screen sizes with built-in button animations and SFX triggers.
* **Memory Management**: Features a dedicated "Reset Progress" system to clear saved data safely.

---

## 🛠️ Technical Stack

* **Engine**: Unity 2022.3+ (compatible with older versions)
* **Language**: C# (Professional/Clean Code)
* **UI System**: TextMeshPro & Unity UI
* **Architecture**: Singleton-lite pattern for GameManager

---

## 📂 Project Structure

* `GameManager.cs`: The brain of the game. Manages levels, scoring, and UI transitions.
* `ButtonEffects.cs`: Handles haptic-like scaling and audio feedback for a better UX.
* `Prefabs/`: Pre-configured button and panel templates.

---

## ⚙️ How to Setup

1.  **Clone the Repo**:
    ```bash
    git clone [https://github.com/YourUsername/WordPuzzlePro.git](https://github.com/YourUsername/WordPuzzlePro.git)
    ```
2.  **Open in Unity**: Add the project folder to your Unity Hub.
3.  **Configure Inspector**: 
    * Drag your letter buttons into the `gameButtons` list.
    * Assign the `settingsPanel` and `nextLevelPanel`.
    * Add your word list in the `wordList` field.

---


## 🤝 Contribution

Contributions are welcome! If you have a feature request or found a bug, please open an issue or submit a pull request.

---

## 👩‍💻 Author

**Maryam**
* Unity Developer & C# Enthusiast
