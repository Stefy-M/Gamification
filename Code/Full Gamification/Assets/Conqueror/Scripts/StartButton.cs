using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Conqueror {
    public class StartButton : MonoBehaviour
    {

        public void Start()
        {
            ChangeSkillText();
            ChangeGunText();
        }

        public void StartGame()
        {
            if (player.Incre.stamina.cur > 0) {
                player.Incre.stamina.cur -= 1;
                GameManager.instance.StartGame();
            }
        }

        public void IncrementStage()
        {
            if (GameManager.instance.workingSave.highestLevelReached < 0)
                GameManager.instance.workingSave.highestLevelReached = 0;

            if (GameManager.instance.level >= GameManager.instance.workingSave.highestLevelReached)
                GameManager.instance.level = 0;
            else
                GameManager.instance.level++;

            GameObject.Find("Selected Level").GetComponent<Text>().text = "Stage " + (GameManager.instance.level + 1);
            Debug.Log("Level = " + GameManager.instance.level);
        }

        public void DecrementStage()
        {
            if (GameManager.instance.workingSave.highestLevelReached < 0)
                GameManager.instance.workingSave.highestLevelReached = 0;

            if (GameManager.instance.level <= 0)
                GameManager.instance.level = GameManager.instance.workingSave.highestLevelReached;
            else
                GameManager.instance.level--;
            
            GameObject.Find("Selected Level").GetComponent<Text>().text = "Stage " + (GameManager.instance.level + 1);
            Debug.Log("Level = " + GameManager.instance.level);
        }

        public void IncrementSkill()
        {
            GameManager.instance.workingSave.currentSkill = Mathf.Clamp(GameManager.instance.workingSave.currentSkill, 0, 2);

            if (GameManager.instance.workingSave.currentSkill >= 2)
                GameManager.instance.workingSave.currentSkill = 0;
            else
                GameManager.instance.workingSave.currentSkill++;

            ChangeSkillText();
            GameManager.instance.Save();
        }

        public void DecrementSkill()
        {
            GameManager.instance.workingSave.currentSkill = Mathf.Clamp(GameManager.instance.workingSave.currentSkill, 0, 2);
            
            if (GameManager.instance.workingSave.currentSkill <= 0)
                GameManager.instance.workingSave.currentSkill = 2;
            else
                GameManager.instance.workingSave.currentSkill--;

            ChangeSkillText();
            GameManager.instance.Save();
        }

        public void IncrementWeapon()
        {
            GameManager.instance.workingSave.currentGun =
                Mathf.Clamp(GameManager.instance.workingSave.currentGun, 
                    0, GameManager.instance.workingSave.guns.Count - 1);

            if (GameManager.instance.workingSave.currentGun >= GameManager.instance.workingSave.guns.Count - 1)
                GameManager.instance.workingSave.currentGun = 0;
            else
                GameManager.instance.workingSave.currentGun++;

            ChangeGunText();
            GameManager.instance.Save();
        }

        public void DecrementWeapon()
        {
            GameManager.instance.workingSave.currentGun =
                Mathf.Clamp(GameManager.instance.workingSave.currentGun,
                    0, GameManager.instance.workingSave.guns.Count - 1);

            if (GameManager.instance.workingSave.currentGun <= 0)
                GameManager.instance.workingSave.currentGun = GameManager.instance.workingSave.guns.Count - 1;
            else
                GameManager.instance.workingSave.currentGun--;

            ChangeGunText();
            GameManager.instance.Save();
        }

        public void DeleteSave()
        {
            GameManager.instance.InitSave();
            GameManager.instance.Save();
        }

        private void ChangeSkillText()
        {
            GameObject.Find("Skill Name").GetComponent<Text>().text = PlayerShip.PlayerSkillToString(GameManager.instance.workingSave.currentSkill);
        }

        private void ChangeGunText()
        {
            GameObject.Find("Weapon Name").GetComponent<Text>().text = "No gun";

            if (GameManager.instance.workingSave.guns.Count > GameManager.instance.workingSave.currentGun)
                GameObject.Find("Weapon Name").GetComponent<Text>().text = GameManager.instance.workingSave.guns[GameManager.instance.workingSave.currentGun].name;
        }
    }
}