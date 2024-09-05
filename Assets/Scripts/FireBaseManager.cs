using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
#if FIREBASE_PLUGIN
using Firebase;
using Firebase.Analytics;
#endif
public class FireBaseManager : MonoBehaviour
{
#if FIREBASE_PLUGIN
    DependencyStatus dependencyStatus = DependencyStatus.UnavailableOther;
#endif
    protected bool firebaseInitialized = false;

    private static FireBaseManager instance;
    public static FireBaseManager Instance { get { return instance; } }

    void Awake ()
	{
        if (FindObjectsOfType(typeof(AdsControl)).Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject); 
    }
	// Use this for initialization
	void Start ()
	{
#if FIREBASE_PLUGIN
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                InitializeFirebase();
            }
            else
            {
                Debug.LogError(
                  "Could not resolve all Firebase dependencies: " + dependencyStatus);
            }
        });
#endif
    }

    // Update is called once per frame
    void Update ()
	{
		
	}

    // Handle initialization of the necessary firebase modules:
    void InitializeFirebase()
    {
#if FIREBASE_PLUGIN
        FirebaseAnalytics.SetAnalyticsCollectionEnabled(true);

        // Set the user's sign up method.
        FirebaseAnalytics.SetUserProperty(
          FirebaseAnalytics.UserPropertySignUpMethod,
          "Google");
        // Set the user ID.
        FirebaseAnalytics.SetUserId("uber_user_510");
        // Set default session duration values.
        FirebaseAnalytics.SetMinimumSessionDuration(new TimeSpan(0, 0, 10));
        FirebaseAnalytics.SetSessionTimeoutDuration(new TimeSpan(0, 30, 0));
        firebaseInitialized = true;
#endif
    }

    public void AnalyticsLogin()
    {
#if FIREBASE_PLUGIN

        // Log an event with no parameters.
       
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventLogin);

#endif
    }

    public void AnalyticsProgress()
    {
#if FIREBASE_PLUGIN

        // Log an event with a float.
      
        FirebaseAnalytics.LogEvent("progress", "percent", 0.4f);

#endif
    }

    public void AnalyticsScore()
    {
#if FIREBASE_PLUGIN

        // Log an event with an int parameter.
      
        FirebaseAnalytics.LogEvent(
          FirebaseAnalytics.EventPostScore,
          FirebaseAnalytics.ParameterScore,
          42);

#endif
    }

    public void AnalyticsGroupJoin()
    {

#if FIREBASE_PLUGIN
        // Log an event with a string parameter.
       
        FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventJoinGroup, FirebaseAnalytics.ParameterGroupId,
          "spoon_welders");

#endif
    }

    public void AnalyticsLevelUp()
    {
#if FIREBASE_PLUGIN

        // Log an event with multiple parameters.
       
        FirebaseAnalytics.LogEvent(
          FirebaseAnalytics.EventLevelUp,
          new Parameter(FirebaseAnalytics.ParameterLevel, 5),
          new Parameter(FirebaseAnalytics.ParameterCharacter, "mrspoon"),
          new Parameter("hit_accuracy", 3.14f));
        
#endif
    }

    // Reset analytics data for this app instance.
    public void ResetAnalyticsData()
    {
#if FIREBASE_PLUGIN
        FirebaseAnalytics.ResetAnalyticsData();
#endif
    }

    public void LogScreen(string _log)
    {
#if FIREBASE_PLUGIN
        FirebaseAnalytics.LogEvent(_log);
#endif
    }

}
