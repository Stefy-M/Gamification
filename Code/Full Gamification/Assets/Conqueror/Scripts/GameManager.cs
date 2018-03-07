using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Conqueror {
    public class GameManager : MonoBehaviour
    {
        public int level = 0;
        public bool startingGame = false;
        public int bossesLeft = 0;
        public ConquerorSave workingSave;

        public static GameManager instance = null;
        public GameObject soundManager;
        public AudioClip music;
        public GameObject[] arenas;
        public GameObject[] bosses;
        public GameObject bulletPrefab;
        public GameObject bossBulletPrefab;

        public GameObject startMenu;
        public GameObject gameUI;
        public PlayerShip playerShip;

        private GameObject currentArena;

        void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(gameObject);

            DontDestroyOnLoad(gameObject);

            if (SoundManager.instance == null)
                Instantiate(soundManager);

            SoundManager.instance.PlayMusic(music);

            InitSave();
            Load();
            Save();
        }
    	
    	void Update()
        {
            if (startingGame && bossesLeft <= 0)
            {
                bossesLeft = 1;
                Invoke("TransitionToNextLevel", 5);
            }

    		// Something that kills this when switching to a different game.
            if (player.Incre.currentGame != minigame.conquer)
                Destroy(gameObject);
        }

        public void InitSave()
        {
            workingSave = new ConquerorSave();
            workingSave.guns = new List<Gun>();
            workingSave.guns.Add(new Gun(1, 30f/111, 20, 0));
            workingSave.currentGun = 0;
            workingSave.currentSkill = 0;
            workingSave.highestLevelReached = 0;
        }

        public void Save()
        {
            player.conqueror = workingSave;
        }

        public void Load()
        {
            if (player.conqueror != null
                && player.conqueror.guns != null
                && player.conqueror.guns.Count > 0)
            {
                workingSave.guns = player.conqueror.guns;
                workingSave.currentGun = player.conqueror.currentGun;
                workingSave.currentSkill = player.conqueror.currentSkill;
                workingSave.highestLevelReached = player.conqueror.highestLevelReached;
            }
        }

        public void AddGun(Gun g)
        {
            if (g == null)
                return;

            if (GameManager.instance.workingSave.guns.Count >= 9)
                workingSave.guns.RemoveAt(0);
            
            workingSave.guns.Add(g);
        }

        public void StartGame()
        {
            startingGame = true;
            currentArena = Instantiate(arenas[level % arenas.Length]) as GameObject;
            playerShip.Init();
            playerShip.gameObject.SetActive(true);
            gameUI.SetActive(true);
            startMenu.SetActive(false);
            Instantiate(bosses[level % bosses.Length]);
            bossesLeft = 1;
        }

        public void GoToMenu()
        {
            startingGame = false;
            gameUI.SetActive(false);
            startMenu.SetActive(true);
            playerShip.gameObject.SetActive(false);
        }

        private void TransitionToNextLevel()
        {
            level++;
            workingSave.highestLevelReached = level;
            player.conqueror = workingSave;
            Destroy(currentArena);
            StartGame();
        }
    }
}