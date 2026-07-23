using UnityEngine;
using UnityEngine.Events;

public class Clock : Singleton<Clock>
{
    public float TimeScale = 1f;

    public bool IsPaused = false;

    public int TicksPerSecond = 60;

    [Header("Events")] public UnityEvent<float> TimeScaleChanged;

    public UnityEvent Paused;
    private float _tickInterval;

    private float _ticksPerMinute;
    // A day has 60,000 ticks making day cycles easy to segment

    private float _tickTimer;
    public float DeltaTime => Time.deltaTime * TimeScale;
    public float FixedDeltaTime => Time.fixedDeltaTime * TimeScale;

    public int TickCount { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        _tickInterval = 1f / (float)TicksPerSecond;
    }

    private void FixedUpdate()
    {
        if (!IsPaused)
        {
            _tickTimer += FixedDeltaTime;

            while (_tickTimer >= _tickInterval)
            {
                _tickTimer -= _tickInterval;
                Tick();
            }
        }
    }

    public void SetTimeScale(float timeScale)
    {
        if (timeScale < 0)
        {
            Debug.LogError("Can't assign the time scale a negative number!");
            return;
        }

        TimeScale = timeScale;

        if (IsPaused)
        {
            IsPaused = false;
        }

        OnTimeScaleChanged(timeScale);
    }

    public void Pause()
    {
        TimeScale = 0f;
        IsPaused = true;
    }

    private void Tick()
    {
        TickCount++;
    }

    private void OnTimeScaleChanged(float newScale)
    {
        if (newScale == 0)
        {
            // Should've paused
            OnPause();
            return;
        }

        if (IsPaused)
        {
            IsPaused = false;
        }

        TimeScaleChanged?.Invoke(newScale);
    }

    private void OnPause()
    {
        Paused?.Invoke();
    }
}