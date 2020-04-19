using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartLevel()
    {
        gameObject.SetActive(true);

        HomeMarker home = GetComponentInChildren<HomeMarker>();

        // forgive me padre for I have sinned
        CharacterPlayer player = GetComponentInChildren<CharacterPlayer>();
        if (player != null)
        {
            player.SetPosition(home.GetPosition());
            player.gameObject.transform.SetParent(home.gameObject.transform.parent);
            Street backstreetsBackAlright = home.gameObject.transform.parent.gameObject.GetComponent<Street>();
            Vector3 temp = player.gameObject.transform.localPosition;
            temp.y = backstreetsBackAlright.StreetYOffset;
            player.gameObject.transform.localPosition = temp;
        }

    }

    public void EndLevel()
    {
        gameObject.SetActive(false);

        //TODO: Open Surgery (haha)
    }
}
