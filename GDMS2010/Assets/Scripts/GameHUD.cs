using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.UI;

public class GameHUD : MonoBehaviour
{
    Slider HealthBar;
    Text HealthText;
    Image HealthBarFill;
    int weaponIndex = 0;
    bool isWeaponListOpen;
    public GameObject curWeapon;
    private GunScript gunProperties;

    int h = 100;
    Inventory inv;
    List<GameObject> weaponList;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        HealthBar = transform.Find("HealthBar").GetComponent<Slider>();
        HealthText = transform.Find("HealthBar").Find("Health").GetComponent<Text>();
        HealthBarFill = transform.Find("HealthBar").Find("Fill Area").Find("Fill").GetComponent<Image>();
        HealthBarFill.color = Color.green;
        weaponList = new List<GameObject>();
        Transform weaponpage = transform.Find("Weapon");
        for (int i = 0; i < weaponpage.childCount; i++)
        {
            weaponList.Add(weaponpage.GetChild(i).gameObject);
        }
        InitWeaponList();
        InitPlayer();
        ReferenceWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (!isWeaponListOpen)
            {
                isWeaponListOpen = true;
                StartCoroutine(OpenWeaponList());
            }
            weaponIndex = (weaponIndex + 1) % 4;
            Debug.Log(weaponIndex);
            weaponList[0].transform.position = weaponList[weaponIndex + 1].transform.position;
        }
        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (!isWeaponListOpen)
            {
                isWeaponListOpen = true;
                StartCoroutine(OpenWeaponList());
            }
            weaponIndex = (weaponIndex - 1) % 4;
            if (weaponIndex < 0)
                weaponIndex += 4;
            Debug.Log(weaponIndex);
            weaponList[0].transform.position = weaponList[weaponIndex + 1].transform.position;
        }
        if (Input.GetMouseButtonDown(0) && isWeaponListOpen)
        {
            StartCoroutine(CloseWeaponList());
            isWeaponListOpen = false;
        }

        if (Input.GetMouseButton(0) && timer < 0)
        {
            inv.weaponList[weaponIndex].Firing();
            timer = 0.05f;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            inv.weaponList[weaponIndex].Reload();
        }
        if (timer >= 0)
            timer -= Time.deltaTime;

        //if (gunProperties)
        //    setCurrentAmmo((int)gunProperties.bulletsIHave);
    }

    public void setCurrentHealth(int value)
    {
        HealthBar.value = value;
        HealthText.text = value.ToString();
        HealthBarFill.color = Color.Lerp(Color.red, Color.green, (float)value / HealthBar.maxValue);
    }

    public void setMaxHealth(int value)
    {
        HealthBar.maxValue = value;
    }

    public void setCurrentAmmo(int value, int _weaponIndex = -1)
    {
        int index = _weaponIndex;
        if (index == -1)
            index = weaponIndex;
        weaponList[index + 1].transform.Find("Cur").GetComponent<Text>().text = value.ToString();

        float amo = (float)inv.weaponList[index].currentAmmo / (float)inv.weaponList[index].MagazineSize;
        weaponList[index + 1].transform.Find("Image").GetComponent<Image>().fillAmount = amo;
    }

    public void setMaxAmmo(int value, int _weaponIndex = -1)
    {
        int index = _weaponIndex;
        if (index == -1)
            index = weaponIndex;
        weaponList[index + 1].transform.Find("Max").GetComponent<Text>().text = value.ToString();
    }

    public void setMaxMagazine(int value, int weaponIndex = -1)
    {

    }

    public void debug_health(int value)
    {
        h += value;
        h = Mathf.Clamp(h, 0, 100);
        setCurrentHealth(h);
        Debug.Log(h);
    }

    IEnumerator OpenWeaponList()
    {
        weaponList[0].SetActive(true);
        float pos = 0.0f;
        while (pos <= 48f)
        {
            for (int i = 1; i < weaponList.Count; i++)
            {
                weaponList[i].SetActive(true);
                weaponList[i].transform.Translate(new Vector3(0, (i - 1) * pos));
            }
            pos += 12.0f;
            yield return null;
        }

    }
    IEnumerator CloseWeaponList()
    {
        float pos = 0.0f;
        while (pos <= 48f)
        {
            for (int i = 1; i < weaponList.Count; i++)
            {
                weaponList[i].SetActive(true);
                weaponList[i].transform.Translate(new Vector3(0, (i - 1) * -pos));
            }
            pos += 12.0f;
            yield return null;
        }
        for (int i = 0; i < weaponList.Count; i++)
        {
            weaponList[i].SetActive(false);
        }
        weaponList[weaponIndex + 1].SetActive(true);

    }

    void InitWeaponList()
    {
        inv = FindObjectOfType<Inventory>();
        if (!inv)
            return;
        for (int i = 0; i < inv.weaponList.Count; i++)
        {
            weaponList[i + 1].transform.Find("Image").GetComponent<Image>().sprite = inv.weaponList[i].icon;
            float amo = (float)inv.weaponList[i].currentAmmo / (float)inv.weaponList[i].MagazineSize;
            weaponList[i + 1].transform.Find("Image").GetComponent<Image>().fillAmount = amo;
            weaponList[i + 1].transform.Find("BG").GetComponent<Image>().sprite = inv.weaponList[i].icon;
            weaponList[i + 1].transform.Find("Cur").GetComponent<Text>().text = inv.weaponList[i].currentAmmo.ToString();
            weaponList[i + 1].transform.Find("Max").GetComponent<Text>().text = inv.weaponList[i].MaxAmmo.ToString();
        }
    }

    void InitPlayer()
    {
        //HealthBar.maxValue = PlayerMaxHealth;
        //HealthBar.value = playerHealth

    }

    void ReferenceWeapon()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GunInventory gInv = player.GetComponent<GunInventory>();
        gInv.SubscribeToWeaponChange(this);
    }

    public void loadNewWeapon(GameObject obj)
    {
        curWeapon = obj;
        //gunProperties = obj.GetComponent<GunScript>();
    }
}
