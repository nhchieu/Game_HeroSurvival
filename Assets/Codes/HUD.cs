using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class HUD : MonoBehaviour
{
    public enum InfoType { Exp,Level,Kill,Time,Health, HealthBoss }
    public InfoType type;
    public static HUD instance;

    Text myText;
    Slider mySlider;

    private void Awake()
    {
        instance = this;
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();
    }

    private void LateUpdate()
    {
        switch (type) {
            case InfoType.Exp:
                float curExp=GameManager.instance.exp;
                float maxExp = GameManager.instance.nextExp[Mathf.Min(GameManager.instance.level,GameManager.instance.nextExp.Length - 1)];
                mySlider.value = curExp/maxExp;
                break;
            case InfoType.Level:
                myText.text = "Level-"+ GameManager.instance.level;
                break;
            case InfoType.Kill:
                myText.text = ""+ GameManager.instance.kill;
                break;
            case InfoType.Time:
                float time = GameManager.instance.gameTime;
                int min = Mathf.FloorToInt( time / 60);
                int sec = Mathf.FloorToInt(time % 60);
                myText.text=string.Format("{0:D2}:{1:D2}",min,sec);
                break;
            case InfoType.Health:
                float health=GameManager.instance.player.Health;
                float maxHealth=GameManager.instance.player.maxHealth;
                mySlider.value = health/maxHealth;
                break;
            case InfoType.HealthBoss:
                float maxhealthboss = 5000f;
                float healthboss = GameManager.instance.boss.health;
                mySlider.value=healthboss/maxhealthboss;
                break;
        }
    }
}
