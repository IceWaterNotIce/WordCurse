using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class LoginPanelController : MonoBehaviour
{
    [SerializeField]
    private TMP_InputField m_UsernameInputField;

    [SerializeField]
    private TMP_InputField m_PasswordInputField;

    [SerializeField]
    private AuthManager m_AuthManager;

    public async void OnSignInButtonClicked()
    {
        string username = m_UsernameInputField.text;
        string password = m_PasswordInputField.text;

        await m_AuthManager.SignInWithUsernamePasswordAsync(username, password);
    }

    public async void OnSignUpButtonClicked()
    {
        string username = m_UsernameInputField.text;
        string password = m_PasswordInputField.text;

        await m_AuthManager.SignUpWithUsernamePasswordAsync(username, password);
    }

    public async void OnSignInAnonymouslyButtonClicked()
    {
        await m_AuthManager.SignInAnonymouslyAsync();
    }

    public async void OnActive()
    {
        await m_AuthManager.SignInCachedUserAsync();
    }
}