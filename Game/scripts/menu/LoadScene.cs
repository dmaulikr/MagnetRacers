using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour {
   
    public AudioSource start_sound;
    public AudioSource other_sound;
  

    public void loadscene(int level)
    {
        if (level == 4)
          start_sound.Play();

        if (level == 1 || level == 2)
            other_sound.Play();
        
        Application.LoadLevel(level);
    }

	public void quit()
    {
        Application.Quit();
    }
}
