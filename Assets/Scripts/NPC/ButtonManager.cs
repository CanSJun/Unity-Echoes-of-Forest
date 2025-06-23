using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UIElements.UxmlAttributeDescription;

public class ButtonManager : MonoBehaviour
{

    

    public void QuestButtonClick() => ChatControl._instance.TalkNpc();

    public void ShopButtonClick() => ChatControl._instance.NPC_SHOP();
    public void ExitButtonClick() => ChatControl._instance.Destroy();
    public void OKButton() => ChatControl._instance.Accept();

    public void NoButton() => ChatControl._instance.NotAccept();
    
}
