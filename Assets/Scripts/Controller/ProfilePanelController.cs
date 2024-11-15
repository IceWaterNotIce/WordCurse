using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class ProfilePanelController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text m_UsernameText;

    [SerializeField]
    private TMP_Text m_EmailText;

    [SerializeField]
    private TMP_Text m_UserIdText;

    [SerializeField]
    private AuthManager m_AuthManager;

    void OnEnable()
    {
        UserData userData = m_AuthManager.GetUserData();
        m_UserIdText.text = userData.userId;
        Debug.Log("User ID: " + userData.userId);
    }

    public void OnSignOutButtonClicked()
    {
        m_AuthManager.SignOut();
    }
}