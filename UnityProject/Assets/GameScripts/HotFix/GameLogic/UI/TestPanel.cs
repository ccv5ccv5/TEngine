using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;
using UnityEngine.UI;

public class TestPanel : MonoBehaviour
{
    private int _count = 0;
    // Start is called before the first frame update
    void Start()
    {
        Log.Debug("TestPanel Start DO Hotfix");

        _count = 0;

        // _scoreLabel = this.transform.Find("Button (Legacy)").GetComponent<Text>();
        // _scoreLabel.text = "Score : 0";

        var restartBtn = this.transform.Find("Button (Legacy)").GetComponent<Button>();
        restartBtn.onClick.AddListener(OnClickBtn);
    }

    private void OnClickBtn()
    {
        _count++;
        var text = this.transform.Find("Text").GetComponent<Text>();
        text.text = $"english : {_count}";

        var text2 = this.transform.Find("Text2").GetComponent<Text>();
        text2.text = $"中文 : {_count}";
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
