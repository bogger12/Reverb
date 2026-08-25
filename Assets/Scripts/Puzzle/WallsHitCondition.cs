

using System.Collections.Generic;
using UnityEngine;

public class WallsHitCondition : PuzzleCondition
{

    public List<SoundSurface> surfaces = new List<SoundSurface>();

    public List<SoundSurface.Material> soundOrder = new List<SoundSurface.Material>();
    public float timeWindowSeconds = 1f;

    private List<SoundSurface.Material> currentSoundsHeard = new List<SoundSurface.Material>();
    private List<float> timeOfHits = new List<float>();


    void Start()
    {
        foreach (SoundSurface surface in surfaces)
        {
            surface.OnSoundCollision += RecieveSound;
        }
    }

    private void RecieveSound(SoundSurface.Material material, GameObject gameObject)
    {
        Debug.Log(string.Format("recieved sound {0}", material));
        currentSoundsHeard.Add(material);
        timeOfHits.Add(Time.time);
        if (currentSoundsHeard.Count >= soundOrder.Count)
        {
            currentSoundsHeard = currentSoundsHeard.GetRange(currentSoundsHeard.Count - soundOrder.Count, soundOrder.Count);
            timeOfHits = timeOfHits.GetRange(currentSoundsHeard.Count - soundOrder.Count, soundOrder.Count);
            // check if soundOrder ends with currentSounds
            for (int i = 0; i < currentSoundsHeard.Count; i++)
            {
                if (currentSoundsHeard[i] != soundOrder[i]) // doesn't end with
                {
                    // reset list
                    currentSoundsHeard = new List<SoundSurface.Material> { material };
                    timeOfHits = new List<float> { Time.time };
                    return;
                }
            }
            if (timeOfHits[^1] - timeOfHits[0] > timeWindowSeconds) return;

            if (currentSoundsHeard.Count == soundOrder.Count) // Is same
            {
                this.OnCompleted();
            }
        }
    }
}
