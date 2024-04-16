using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_ADAPTIVE_PERFORMANCE_SAMSUNG_ANDROID
using UnityEngine.AdaptivePerformance.Samsung.Android;
#endif

public class VariableRefreshRateControl : MonoBehaviour
{
    public Dropdown supportedRefreshRates;
    public Text currentRefreshRate;
    public Slider targetRefreshRate;
    public Text targetRefreshRateLabel;
    public GameObject notSupportedPanel;
    // How long to run the test (in seconds)
    public float timeOut = 80;

#if UNITY_ADAPTIVE_PERFORMANCE_SAMSUNG_ANDROID
    bool didVRRSupportChange = false;
#endif

    float timeOuttimer = 0;

    void Start()
    {
        timeOuttimer = timeOut;

        Application.targetFrameRate = 60;
        targetRefreshRate.SetValueWithoutNotify(60);

#if UNITY_ADAPTIVE_PERFORMANCE_SAMSUNG_ANDROID
        if (VariableRefreshRate.Instance == null)
        {
            Debug.Log("[AP VRR] Variable Refresh Rate is not supported on this device.");
            notSupportedPanel.SetActive(true);
            return;
        }

        VariableRefreshRate.Instance.RefreshRateChanged += UpdateDropdown;
        supportedRefreshRates.onValueChanged.AddListener(delegate {
            if (!VariableRefreshRate.Instance.SetRefreshRateByIndex(supportedRefreshRates.value))
                UpdateDropdown();
        });
#else
        notSupportedPanel.SetActive(true);
#endif

        targetRefreshRate.onValueChanged.AddListener(delegate {
            Application.targetFrameRate = (int)targetRefreshRate.value;
        });
        UpdateDropdown();
    }

    void Update()
    {
#if UNITY_ADAPTIVE_PERFORMANCE_SAMSUNG_ANDROID
        notSupportedPanel.SetActive(VariableRefreshRate.Instance == null);
#endif
        targetRefreshRateLabel.text = $"Target Refresh Rate: {Application.targetFrameRate} Hz";

        timeOuttimer -= Time.deltaTime;

        if (timeOuttimer < 0)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
#if UNITY_ADAPTIVE_PERFORMANCE_SAMSUNG_ANDROID
        if (VariableRefreshRate.Instance == null)
        {
            UpdateDropdown();
            didVRRSupportChange = true;
            currentRefreshRate.text = $"Current refresh rate: - Hz";
            return;
        }
        else
        {
            if (didVRRSupportChange)
            {
                didVRRSupportChange = false;
                UpdateDropdown();
            }
        }
        currentRefreshRate.text = $"Current refresh rate: {VariableRefreshRate.Instance.CurrentRefreshRate} Hz";
#endif
    }

    void UpdateDropdown()
    {
        var options = new List<Dropdown.OptionData>();
        supportedRefreshRates.ClearOptions();
        var index = -1;

#if UNITY_ADAPTIVE_PERFORMANCE_SAMSUNG_ANDROID
        if (VariableRefreshRate.Instance == null)
            return;

        for (var i = 0; i < VariableRefreshRate.Instance.SupportedRefreshRates.Length; ++i)
        {
            var rr = VariableRefreshRate.Instance.SupportedRefreshRates[i];
            options.Add(new Dropdown.OptionData(rr.ToString()));
            if (rr == VariableRefreshRate.Instance.CurrentRefreshRate)
                index = i;
        }
#endif
        supportedRefreshRates.AddOptions(options);
        supportedRefreshRates.SetValueWithoutNotify(index);
    }
}
