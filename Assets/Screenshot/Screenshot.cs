// ******  Notice : It doesn't works in Wep Player environment.  ******
// ******    It works in PC environment.                         ******
// Default method have some problem, when you take a Screen shot for your game. 
// So add this script.
// CF Page : http://technology.blurst.com/unity-jpg-encoding-javascript/
// made by Jerry ( sdragoon@nate.com )
 
using UnityEngine;
using System.Collections;
using System.IO;
 
public class Screenshot : MonoBehaviour
{
    private int count = 0;
	private int promCount = 0;
	private float clickCount = 1.0f;
 	private GameObject cameraOverlay;
	private ArrayList promis;
	private ArrayList textures;
	private ArrayList found;
	private GameObject finishScreen;
	public bool stabilized = false;
	private GUIStyle TextStyle = new GUIStyle();
	private GUIStyle DescrTextStyle = new GUIStyle();
	private CharacterMotor characterMotor;
	private Hashtable promiTable = new Hashtable();
	private Hashtable newsTable = new Hashtable();
	private bool newsPaper = false;
	private GameObject currNewsPaper;
	private Texture currTexture;
	public string infoText = "";
	private float currTime;
	private bool mouseRelease = true;
	private string[] names = {"Altair", "Batman", "Clown", "Gordon", "Harry", "Rikku", "Robin", "Superman"};
	
	void Start()
	{
		Screen.showCursor = false;
		cameraOverlay = GameObject.Find ("cameraOverlay");
		finishScreen = GameObject.Find ("finishScreen");
		finishScreen.SetActive(false);
		characterMotor = GameObject.Find ("First Person Controller").GetComponent<CharacterMotor>();
		//currNewsPaper.SetActive(false);
		promis = new ArrayList();
		promis.Add (GameObject.Find ("Superman"));
		promis.Add (GameObject.Find ("Robin"));
		promis.Add (GameObject.Find ("Batman"));
		promis.Add (GameObject.Find ("Rikku"));
		promis.Add (GameObject.Find ("Altair"));
		promis.Add (GameObject.Find ("Harry"));
		promis.Add (GameObject.Find ("Gordon"));
		promis.Add (GameObject.Find ("Clown"));
		textures = new ArrayList();
		found = new ArrayList();
		
		
		newsTable.Add ("Altair", GameObject.Find ("Altair_news"));
		newsTable.Add ("Batman", GameObject.Find ("Batman_news"));
		newsTable.Add ("Clown", GameObject.Find ("Clown_news"));
		newsTable.Add ("Gordon", GameObject.Find ("Gordon_news"));
		newsTable.Add ("Harry", GameObject.Find ("Harry_news"));
		newsTable.Add ("Rikku", GameObject.Find ("Rikku_news"));
		newsTable.Add ("Robin", GameObject.Find ("Robin_news"));
		newsTable.Add ("Superman", GameObject.Find ("Superman_news"));
		
		
		
		
		
		foreach (DictionaryEntry pair in newsTable)
        {
			Debug.Log(pair.Key);
			 (pair.Value as GameObject).SetActive(false);
        }
		TextStyle.normal.textColor = Color.red;
		TextStyle.fontStyle = FontStyle.Bold;
		TextStyle.fontSize = 18;
		TextStyle.alignment = TextAnchor.MiddleCenter;
		DescrTextStyle.normal.textColor = Color.white;
		DescrTextStyle.fontStyle = FontStyle.Normal;
		DescrTextStyle.fontSize = 18;
		DescrTextStyle.alignment = TextAnchor.MiddleCenter;
	}
    void Update()
    {
		if(cameraOverlay == null)
		{
			cameraOverlay = GameObject.Find ("cameraOverlay");
			
		}
		else if (cameraOverlay.activeSelf && stabilized && (characterMotor.inputMoveDirection == Vector3.zero) && Input.GetButton ("Fire1") && clickCount >= 0.0f && mouseRelease)
		{
			infoText = "Say Cheese.. "+clickCount.ToString("F2");
			clickCount -= 0.01f;
		}
		else if (cameraOverlay.activeSelf && (characterMotor.inputMoveDirection != Vector3.zero) && Input.GetButton ("Fire1"))
		{
			infoText = "Stand still!";
		}
		else if (cameraOverlay.activeSelf && (characterMotor.inputMoveDirection == Vector3.zero) && Input.GetButton ("Fire1") && !stabilized)
		{
			infoText = "Stabilize ('H') first!";
		}
		else if(!Input.GetButton ("Fire1")) 
		{
			mouseRelease = true;
			if(!stabilized) infoText = "";
			clickCount = 1.0f;
		}
		else if(!cameraOverlay.activeSelf && Input.GetButton ("Fire1"))
		{
			clickCount = 1.0f;
			infoText = "Take out the cam ('C')!";
		}
			
		//Debug.Log (cameraOverlay.name);
        if (clickCount <= 0.0f && cameraOverlay.activeSelf)
		{
			clickCount = 1.0f;
			audio.Play ();
			//cameraOverlay.SetActive (false);
            int i = 0;
			int rem = -1;
			foreach(GameObject promi in promis)
			{
				if(promi.renderer.isVisible)
				{
					float distance = Vector3.Distance(promi.transform.position, gameObject.transform.position);
	    			if(distance <= 15.0f)
					{
						StartCoroutine(ScreenshotEncode(promi));
						rem = i;
					}
					
				}
				else 
				{
					StartCoroutine(ScreenshotEncode(null));
				}
				i++;
			}
			
			if(rem != -1)
			{
				infoText = "Nice picture!";
				promis.RemoveAt(rem);
			}
			else infoText = "Nothing to look at..";
			
			mouseRelease = false;
			//else if (promis.Count == 0) cameraOverlay.SetActive (true);
		}
    }
	
	void OnGUI()
	{
		/*int offset = 0;
		int cnt = 0;
		GUI.Label (new Rect (Screen.width/2, 20, 50, 20), ""+found.Count+"/"+(found.Count+promis.Count)+"", TextStyle);
		GUI.Label (new Rect (50, Screen.height-40, 50, 20), "find Altair, Batman, Gordon Freeman, Harry Potter, Ludwig The Clown, Rikku, Robin and Superman", DescrTextStyle);
		foreach (Texture t in textures)
		{
			GUI.Label (new Rect (10, 10+offset, 100, 100), (Texture)t);
			GUI.Label (new Rect (120, 30+offset, 100, 40), found[cnt] as string, TextStyle);
			offset += 100;
			cnt++;
		}*/
		
		if(Time.time >= 10 && !newsPaper)
		{
			if(currNewsPaper != null)
			{
				currNewsPaper.SetActive(false);
			}
			GUI.Label (new Rect (Screen.width/2-100, 40, 50, 20), infoText, TextStyle);
			
			int offset = 0;
			int cnt = 0;
			if (promis.Count > 0)
				GUI.Label (new Rect (Screen.width/2-100, 20, 50, 20), ""+promiTable.Count+"/"+(promiTable.Count+promis.Count)+"", TextStyle);
			else 
			{
				GUI.Label (new Rect (Screen.width/2-100, 20, 50, 20), ""+promiTable.Count+"/"+(promiTable.Count+promis.Count)+"", DescrTextStyle);
				cameraOverlay.SetActive (false);
				if(!newsPaper)
					finishScreen.SetActive (true);
			}
			//GUI.Label (new Rect (50, Screen.height-40, 50, 20), "find Altair, Batman, Gordon Freeman, Harry Potter, Ludwig The Clown, Rikku, Robin and Superman", DescrTextStyle);
			foreach (string s in names)
			{
				if(cnt <= 3)
				{
					if(promiTable.ContainsKey (s))
					{
						GUI.Label (new Rect (600+offset, 10, 80, 80), (Texture)promiTable[s]);
						GUI.Label (new Rect (600+offset, 100, 80, 20), s, DescrTextStyle);
					}
					else GUI.Label (new Rect (600+offset, 100, 80, 20), s, TextStyle);
					offset += 100;
				}
				else 
				{
					if(cnt == 4) offset = 0;
					
					if(promiTable.ContainsKey (s))
					{
						GUI.Label (new Rect (20+offset, Screen.height-130, 80, 80), (Texture)promiTable[s]);
						GUI.Label (new Rect (20+offset, Screen.height-40, 80, 20), s, DescrTextStyle);
					}
					else GUI.Label (new Rect (20+offset, Screen.height-40, 80, 20), s, TextStyle);
					offset += 100;
				}
				cnt++;
			}
			/*foreach (Texture t in textures)
			{
				GUI.Label (new Rect (10, 10+offset, 100, 100), (Texture)t);
				GUI.Label (new Rect (120, 30+offset, 100, 40), found[cnt] as string, TextStyle);
				offset += 100;
				cnt++;
			}*/
		}
		else if (Time.time >= 10)
		{
			//if(Time.frameCount > currTime+200) 
			if(Time.time > currTime+3)
			{
				newsPaper = false;
			}
			else if(cameraOverlay.activeSelf) cameraOverlay.SetActive(false);
			GUI.Label (new Rect(170, 370, 280, 188), (Texture)currTexture);
		}
	}
 
    IEnumerator ScreenshotEncode(GameObject prom)
    {
        // wait for graphics to render
        yield return new WaitForEndOfFrame();
 		
        // create a texture to pass to encoding
        //Texture2D texture = new Texture2D(512, 418, TextureFormat.RGB24, false);
		//Debug.Log ("width: "+Screen.width/2+", height: "+(Screen.height/2));
		//Texture2D texture = new Texture2D(Screen.width-100, Screen.height, TextureFormat.RGB24, false);
		Texture2D texture = new Texture2D(320, 250, TextureFormat.RGB24, false);
 		//Texture2D texture = new Texture2D((int)Mathf.Round ((float)(Screen.width/2)), (int)Mathf.Round ((float)(Screen.height/1.837)), TextureFormat.RGB24, false);
        // put buffer into texture
		texture.ReadPixels(new Rect(145, 250, 320, 250), 0, 0);
        //texture.ReadPixels(new Rect(460, 170, 512, 418), 0, 0);
		//texture.ReadPixels (new Rect(50, 0, Screen.width-50, Screen.height), 0,0);
		//texture.ReadPixels(new Rect((int)Mathf.Round ((float)(Screen.width/2.226)), (int)Mathf.Round ((float)(Screen.height/4.517)), (int)Mathf.Round ((float)(Screen.width/2)), (int)Mathf.Round ((float)(Screen.height/1.837))), 0, 0);
        texture.Apply();
 		//cameraOverlay.SetActive (true);
        // split the process up--ReadPixels() and the GetPixels() call inside of the encoder are both pretty heavy
        yield return 0;
 
        byte[] bytes = texture.EncodeToPNG();
 
        // save our test image (could also upload to WWW)
		if(prom != null)
		{
        	File.WriteAllBytes(Application.dataPath + "/../pictures/promi-" + promCount + ".png", bytes);
        	promCount++;
			//texture.Resize (100,100);
			texture.Compress (false);
			//textures.Add (texture);
			promiTable.Add (prom.name, texture);
			found.Add (prom.name);
			currNewsPaper = (GameObject)newsTable[prom.name];
			currNewsPaper.SetActive(true);
			currTexture = texture;
			Debug.Log (prom.name + "_news");
			newsPaper = true;
			//currTime = Time.frameCount;
			currTime = Time.time;
		}
		else
		{
			File.WriteAllBytes(Application.dataPath + "/../pictures/picture-" + count + ".png", bytes);
        	count++;
			DestroyObject( texture );
		}
 		
        // Added by Karl. - Tell unity to delete the texture, by default it seems to keep hold of it and memory crashes will occur after too many screenshots.
        //DestroyObject( texture );
 
        //Debug.Log( Application.dataPath + "/../testscreen-" + count + ".png" );
		
    }
}