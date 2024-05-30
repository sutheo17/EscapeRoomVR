# EscapeRoomVR
Egy VR játék amelyet az Önálló laboratórium tárgyra készítettem el. 4 szobából áll, mindegyik egy másik kihívást tartogat a játékos számára.

*Konzulens: Hideg Attila*

# Dokumentáció

A játékban szereplő Asset-ek mind az Unity-s Asset Shopból kerültek letöltésre.

Alapvetően a játék a következő komponensekből épül fel:
- Lighting
- Interface
- Environment
- XR
- Level 1
- Level 2
- Level 3
- Level 4

A következőkben ezen komponenseket szeretném részletesebben kifejteni, illetve a hozzájuk kapcsolodó szkripteket elmagyarázni.

![Program structure](Komponensek.png)

## Lighting

A játék megvilágításáért felelős, mndegyik GameObject függ tőle. A program alapvetően Mixed megvilágítást használ, a statikus objektumok Baked Lighting-al jelennek meg (lényegében már fordítási időben legenerálódnak az árnyékok, "beleégnek" az objektumokba), illetve Real-time megvilágítást használnak az interaktálható, dinamikus objektumok. Ezzel is a teljesítményt növeljük. Van egy Directional Light, illetve ezen kívül néhány Spot / Point Light ami vagy az egyes objektumok funkcionalítását adja vagy az egyes szobák megvilágításáért felelős.

## Environment

Alapvetően csak a külső atmoszféra megteremtéséért felelős. Fontos, hogy alacsony vertex-számú GameObjectek alkotják, hogy ezzel is a teljesítményt növelni tudjuk.

Három szkript van itt az atmoszféra megteremtésére, a **PlayContinuousSound**, a **PlayRandomSounds** és a **PlayQuickSound**

```C#
//PlayContinuousSound

private void Start()
{
    if (playOnStart)
        Play();
}

public void Play()
{
    audioSource.clip = sound;
    audioSource.Play();
}
```

```C#
//PlayRandomSounds

 void Start()
 {
     // Start the coroutine to play sounds randomly
     StartCoroutine(PlayRandomBirdSound());
 }


 private IEnumerator PlayRandomBirdSound()
 {
     while (true)
     {
         // Randomly select a time interval between 10 and 20 seconds
         float waitTime = Random.Range(10f, 20f);
         yield return new WaitForSeconds(waitTime);

         // Randomly select one of the birds
         int birdIndex = Random.Range(0, 3);

         // Play sound on the selected bird
         switch (birdIndex)
         {
             case 0:
                 bird_1.GetComponent<PlayQuickSound>().Play();
                 break;
             case 1:
                 bird_2.GetComponent<PlayQuickSound>().Play();
                 break;
             case 2:
                 bird_3.GetComponent<PlayQuickSound>().Play();
                 break;
         }

     }
 }
```

```C#
//PlayQuickSound

private void Awake()
{
    audioSource = GetComponent<AudioSource>();
}

public void Play()
{
    float randomVariance = Random.Range(-randomPitchVariance, randomPitchVariance);
    randomVariance += defaultPitch;

    audioSource.pitch = randomVariance;
    audioSource.PlayOneShot(sound, volume);
    audioSource.pitch = defaultPitch;
}
```

Alapvetően egy szél hang megy folyamatosan loopolva, illetve 10-20 másodpercenként egy random madárcsicsergés is felhallatszódik amint elkezdődik a játék. A **PlayQuickSound** segítségével játszuk le a rövid hangeffekteket.

## Interface

Ennek a komponensek több fontos feladata is van:
- A játékba való betöltéskor átvezetés (5 másodperc alatt kerülünk sötétségből a virtuális világba)
  
  Ez a **FadeCanvas** szkript segítségével történik
  
```C#  
//FadeCanvas

private IEnumerator FadeIn(float duration)
{
    float elapsedTime = 0.0f;

    while (alpha <= 1.0f)
    {
        SetAlpha(elapsedTime / duration);
        elapsedTime += Time.deltaTime;
        yield return null;
    }
}

private IEnumerator FadeOut(float duration)
{
    float elapsedTime = 0.0f;

    while (alpha >= 0.0f)
    {
        SetAlpha(1 - (elapsedTime / duration));
        elapsedTime += Time.deltaTime;
        yield return null;
    }
}
```
Alapvetően az alpha-val a Canvas objektum átlátszóságát állítjuk be, és szépen az eltelt idővel arányosan tűntetjük el szépen fokozatosan a Canvas-t a felhasználó elől. A játék elején egy teljesen fekete Canvas tűnik el, ezzel megkönnyítve a virtuális világba való belépést.

Ezt pedig a SceneLoader GameObject hívja meg amikor elindul a játék, ezért a **OnSceneLoad** szkript felelős:

```C#  
//OnSceneLoad

public UnityEvent OnLoad = new UnityEvent();

 private void Awake()
 {
     SceneManager.sceneLoaded += PlayEvent;
 }

 private void OnDestroy()
 {
     SceneManager.sceneLoaded -= PlayEvent;
 }

 private void PlayEvent(Scene scene, LoadSceneMode mode)
 {
     OnLoad.Invoke();
 }
 ```  
 
 Ez feliratkozik a SceneManager eventjére ami akkor hívodik meg amikor betölt a Scene, és szépen elsüti ezt az OnLoad UnityEventet.

 Az összes pálya Clue, illetve Progress felirat megjelenítése is a **FadeCanvas** szkript segítségével történik, ezeket a pályákhoz tartozó szkriptek fogják triggerelni.
 
 Az üdvözlő felirat az egy XR UI Canvas, így ezzel a játékos majd tud interaktálni a Ray Interactor ("nyaláb") segítségével majd (erre az XR részben részletesebben is kitérek). Amikor itt megnyomjuk a gombot meghívodik az első Clue a játékos számára az gomb OnClick event hatására (a Level1Clue GameObject FadeCanvas.FadeIn függvénye).

 Az egyes biztonsági triggerek logikája is itt van megvalósítva. Érzékeljük egy Box Colliderrel azt, hogy egy dinamikus objektum vagy maga a játékos leesik e pályáról, és ekkor a **FallOfTrigger** szkript fut le, ami meghívja a **LoadScene** szkript függvényét, ami a pálya újraindításáért felelős.

 ```C#
//FallOfTrigger

 public void OnTriggerEnter(Collider other)
{
    GetComponent<LoadScene>().ReloadCurrentScene();
}
```

```C#
//LoadScene

public void ReloadCurrentScene()
{
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
```

 ## Level 1

 Az első pálya alapvetően egy "hagyományos" szabadulószoba jellegű. Az itt lévő Clue a következő: "Picture yourself in the other room".
 Ezzel utalva arra, hogy a megoldás a kép keret mögött lesz. Ezután a megfelelő kódot megadva a játékos kap egy Keycard-ot, ami ha az olvasónál lehúz, akkor már el tudja húzni az ajtót a következő szobába.

Itt a két képen van egy **Painting** szkript:

```C#
//Painting

void Start()
{
    var obj = Painting_object.GetComponent<XRGrabInteractable>();

    rb = obj.GetComponent<Rigidbody>();
    
    rb.constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

    obj.selectEntered.AddListener(OnGrab);
}

private void OnGrab(SelectEnterEventArgs interactor)
{
    rb.constraints = RigidbodyConstraints.None;
}
```

Alapvetően ezek a GameObjectek rendelkeznek az XRGrabInteractable komponenssel, ezért hatna rájuk a gravitáció, alapból kikapcsoljuk ezért a rájuk ható gravitációt, és amint a player felveszi őket, akkor visszaállítjuk a rájuk ható gravitációt.

A keypad egyes gombjain egy **TouchButton** szkript van (valamint egy **PlayQuickSound** a gombnyomás hangjára):

```C#
//TouchButton

protected override void OnHoverEntered(HoverEnterEventArgs interactor)
{
    base.OnHoverEntered(interactor);
    var renderer = button.GetComponent<Renderer>();
    oldColor = renderer.material.color;
    numberPad = GetComponentInParent<NumberPad>();

    var sound = button.GetComponent<PlayQuickSound>();
    sound.Play();

    numberPad.EnterDigit(digit);

    renderer.material.color = Color.green;
}

protected override void OnHoverExited(HoverExitEventArgs interactor)
{
    base.OnHoverExited(interactor);
    var renderer = button.GetComponent<Renderer>();
    renderer.material.color = oldColor;
}
```

Ez alapvetően annyit tesz, hogy amikor a játékos hozzáér egy gombhoz, akkor amíg érintkezik vele zölddé színezi, amikor már nem akkor visszaállítja a régi színére. Illetve, a **NumberPad** szkript EnterDigit függvényét meghívja.

```C#
//NumberPad

public void EnterDigit(string number)
{
    if(!waitingForReset && !alreadyGaveKeycard)
    {
        if(number == "back")
        {
            if(codeForNow.Length > 0)
            {
                codeForNow = codeForNow.Substring(0, codeForNow.Length - 1);
                text.text = codeForNow;
            }
            
        }
        else if(number == "clear")
        { 
            text.color = Color.blue;
            text.text = "Resetting code";
            waitingForReset = true;
            StartCoroutine(ResetTextAfterDelay(2f));
        }
        else //is a real digit
        {
            codeForNow += number;
            text.text = codeForNow;
            if (codeForNow.Length == 4)
            {
                originalTextColor = text.color;
                if (codeForNow == code)
                {
                    text.color = Color.green;
                    text.text = "Code is correct!";
                    Instantiate(keyCard, attachPoint.transform.position, attachPoint.transform.rotation);
                    var sound = this.GetComponent<PlayDoorSound>();
                    sound.PlayCorrect();
                    alreadyGaveKeycard = true;
                    StartCoroutine(ResetTextAfterCorrectCodeDelay(4f));

                }
                else
                {
                    text.color = Color.red;
                    text.text = "Code is invalid!";
                    var sound = this.GetComponent<PlayDoorSound>();
                    sound.PlayWrong();
                    waitingForReset = true;
                    StartCoroutine(ResetTextAfterDelay(4f));
                }
            }
        }
    }
    
}

private IEnumerator ResetTextAfterDelay(float delay)
{
    // Wait for the specified delay
    yield return new WaitForSeconds(delay);

    // Reset the text and color
    text.color = originalTextColor;
    text.text = "";
    codeForNow = "";
    waitingForReset = false;
}

private IEnumerator ResetTextAfterCorrectCodeDelay(float delay)
{
    // Wait for the specified delay
    yield return new WaitForSeconds(delay);

    // Reset the text and color
    text.color = originalTextColor;
    text.text = "Pick up the keycard from below!";
}
```

Alapvetően itt egy négy karakteres jelszót várunk, ha nem jó akkor ezt kijelezzük addig letiltjuk a karakterek beírását. Ha pedig jó, akkor lespawnolunk egy KeyCard GameObjectet. Ezután már az inputok nem kerülnek beolvasásra. A NumberPad-hez kapcsolódik még egy **PlayDoorSound** szkript:

```C#
//PlayDoorSound

void Awake()
{
    m_AudioSource = GetComponent<AudioSource>();
}

public void PlayCorrect()
{
    float randomVariance = Random.Range(-m_RandomPitchVariance, m_RandomPitchVariance);
    randomVariance += m_DefaultPitch;

    m_AudioSource.pitch = randomVariance;
    m_AudioSource.PlayOneShot(m_SoundCorrect, m_Volume);
    m_AudioSource.pitch = m_DefaultPitch;
}

public void PlayWrong()
{
    float randomVariance = Random.Range(-m_RandomPitchVariance, m_RandomPitchVariance);
    randomVariance += m_DefaultPitch;

    m_AudioSource.pitch = randomVariance;
    m_AudioSource.PlayOneShot(m_SoundWrong, m_Volume);
    m_AudioSource.pitch = m_DefaultPitch;
}
```
Alapvetően nagyon hasonló a PlayQuickSound-hoz, de itt kétféle hangfájlt tárolunk el, és játszuk le a megfelelő az egyes kódpróbálkozásoknál.

A KeyCard-ot ezek után a leolvason le kell húzni, ennek a validálásáért a **CardReader** szkript felelős:

```C#
//CardReader

protected override void OnHoverEntered(HoverEnterEventArgs args)
{
    base.OnHoverEntered(args);

    m_KeycardTransform = args.interactableObject.transform;
    m_HoverEntry = m_KeycardTransform.position;
    m_SwipIsValid = true;
}

protected override void OnHoverExited(HoverExitEventArgs args)
{
    base.OnHoverExited(args);

    Vector3 entryToExit = m_KeycardTransform.position - m_HoverEntry;

    if (m_SwipIsValid && entryToExit.y < -0.15f)
    {
        VisualLockToHide.gameObject.SetActive(false);
        HandleToEnable.GetComponent<SlidingDoor>().SetReady();
        GetComponent<PlayQuickSound>().Play();
    }

    m_KeycardTransform = null;
}

private void Update()
{
    if (m_KeycardTransform != null)
    {
        Vector3 keycardUp = m_KeycardTransform.forward;
        float dot = Vector3.Dot(keycardUp, Vector3.up);

        if (dot < 1 - AllowedUprightErrorRange)
        {
            m_SwipIsValid = false;
        }
    }
}
```
Alapvetően az Update függvényben ellenőrizzük, hogy a KeyCard megfelelően áll (ezt egy skaláris szorzással validáljuk). A lehúzást egy Trigger érzékeli, és ha megfelelőne áll és megfelelő távolságig lehúztuk a kártyát akkor az ajtóról lekerül a zár és egy hangeffekt jelzi, hogy jól végeztük a dolgunkat.

Az ajtó kiléncsen van egy **Sliding Door** szkript:

```C#
 private void Start()
 {
     DragDistance = 0.8f;
     DraggedTransform = Door.transform;
     LocalDragDirection = new Vector3(-1, 0, 0);
     m_WorldDragDirection = transform.TransformDirection(LocalDragDirection).normalized;
    

     m_StartPosition = DraggedTransform.position;
     m_EndPosition = m_StartPosition + m_WorldDragDirection * DragDistance;
 }

 public void SetReady()
 {
     setReady = true;
 }

 public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
 {
     if (isSelected)
     {
         if(setReady)
         {
             UnityEngine.Debug.Log("Door is ready to be dragged");
             var interactorTransform = firstInteractorSelecting.GetAttachTransform(this);
             Vector3 selfToInteractor = interactorTransform.position - transform.position;

             // calculate dot product of selfToInteractor onto drag direction
             float force = Vector3.Dot(selfToInteractor, m_WorldDragDirection);

             // calculate speed based the dot product
             float absoluteForce = Mathf.Abs(force);
             float speed = absoluteForce / Time.deltaTime / DoorWeight;
             // move door based on speed using MoveTowards
             UnityEngine.Debug.Log("Dragged: " + DraggedTransform.position);
             DraggedTransform.position = Vector3.MoveTowards(DraggedTransform.position, m_EndPosition, speed * Time.deltaTime);
         }
        
     }
 }
 ```

Ez a szkript alapvetően azt nézi meg, hogy a játékos honnan próbálja arébb húzni az ajtót. Ha merőlegesen áll az ajóhoz képest, akkor az ajó nem fog megmozdulni. Azonban ha arébb áll kicsit, és megfelelő erővel húzza az ajtót akkor az arébb fog menni egy előre kijelölt tengelyen (a mi esetünkben Vec3(-1,0,0)). Meg van adva a maximális kinyílás is. A többi igazából már csak megfelelő konstansok hozzáadása, hogy élethű legyen, az ajótnak legyen "súlya".

Ezek után átlépünk a második pályára.

 ## Level 2

Itt a játékos feladata nem más lesz mint a rendrakás. A pálya Clue-ja a következő: "Let us, above all things, be neat and orderly."
Háromféle dolgot kell a játékosnak a helyére raknia:
- könyvek
- kalapok
- sportszerek
A könyvekből három darab van, mind különböző színűek és ezt a könyvespolcon lévő Socket-ekre kell helyezni. Az egyes socket-eken van egy **Hook** szkript:

```C#
//Hook

public void Awake()
{
    GetComponent<XRSocketInteractor>().selectEntered.AddListener(OnSelectEnter);
}

public void OnSelectEnter(SelectEnterEventArgs args)
{
    // Get the interactable object that was placed in the socket
    XRBaseInteractable interactable = (XRBaseInteractable)args.interactableObject;
    if (interactable != null)
    {
        if (type == 1)
        {
            if (color == "red")
            {
                if (interactable.gameObject.name.Contains("Red"))
                {
                    StartCoroutine(FreezeAfterDelay(interactable.gameObject));
                }

            }
            else if (color == "blue")
            {
                if (interactable.gameObject.name.Contains("Blue") && !interactable.gameObject.name.Contains("Brown"))
                {
                    StartCoroutine(FreezeAfterDelay(interactable.gameObject));
                }
            }
            else //brown
            {
                if (interactable.gameObject.name.Contains("Brown"))
                {
                    StartCoroutine(FreezeAfterDelay(interactable.gameObject));
                }
            }
        }
        else
        {
            // Start the coroutine to freeze the item after the transformation
            StartCoroutine(FreezeAfterDelay(interactable.gameObject));
        }
    }
  
    
}

private IEnumerator FreezeAfterDelay(GameObject attachedItem)
{
    // Wait for the end of the frame to ensure all transformations are applied
    yield return new WaitForSeconds(0.5f);

    // Disable the XRGrabInteractable component on the attached item
    XRGrabInteractable grabInteractable = attachedItem.GetComponent<XRGrabInteractable>();
    this.TryGetComponent(out SphereCollider sphereCollider);
    this.TryGetComponent(out BoxCollider boxCollider);
    if(type == 0)
    {
        sphereCollider.enabled = false;
    }
    else
    {
        boxCollider.enabled = false;
    }

    if (grabInteractable != null)
    {
        grabInteractable.enabled = false;
    }

    // Freeze the attached item in place by setting Rigidbody constraints
    Rigidbody itemRigidbody = attachedItem.GetComponent<Rigidbody>();
    if (itemRigidbody != null)
    {
        itemRigidbody.isKinematic = true;
        itemRigidbody.constraints = RigidbodyConstraints.FreezeAll;
    }

    var level = parent.GetComponentInParent<Level2Progress>();

    if(type == 0)
    {
        level.HatPlaced();
    }
    else
    {
        level.BookPlaced();
    }
    
}
```

Ez a szkript fog visszatérni majd az akasztóknál is (ahova a kalapok kerülnek majd). Itt annyi, hogy a szkript *type* változójának 1-est állítunk be. Alapvetően amikor egy GameObject belép a BoxColliderünk Triggerjébe, akkor megnézzük, hogy az adott helyen milyen színnek kéne lennie, és hogy milyen színű könyv került fel ide (ez a GameObject nevéből kiderül). Ha megfelelő könyv van az adott helyen akkor meghívjuk **Level2Progress** szkript BookPlaced függvényét, valamint nem interaktálhatóvá állítjuk a könyvet, hogy a játékos ne tudja arébb rakni.

Az akasztók és kalapok esetében is a fenti szkript felelős a logika megvalósításáért. Itt annyi a különbség, hogy a *type* változót 0-ra állítjuk, illetve az, hogy itt nincs megfelelő sorrend. Ha egy sapka felkerül a Socket-re akkor meghívódik a **Level2Progress** HatPlaced függvénye.

A szobában található egy doboz amibe pedig a játékszereket kell rakni. Összesen 3 sport labdája és ütője (tenisz, rögbi, ping-pong) található meg a szobában. Ezeket kell a dobozba helyezni, a behelyezett GameObject-eket a **Storage** szkript kezeli:

```C#
private List<string> alreadyIn = new List<string>();
private void OnTriggerEnter(Collider other)
{
    XRGrabInteractable gameObject = other.GetComponent<XRGrabInteractable>();
    if (gameObject != null && !alreadyIn.Contains(other.name))
    {
        Debug.Log(gameObject.interactionLayers.value);
        if (gameObject.interactionLayers.value != 2 && gameObject.
        //32 -> SportItem
        interactionLayers.value != 32)
        {
            alreadyIn.Add(gameObject.name);

            other.gameObject.SetActive(false);
            var level = GetComponentInParent<Level2Progress>();
            level.SportItemPlaced();
            gameObject.enabled = false;
        }
        else
        {
            // Convert comma-separated ranges to period-separated ranges
            float minX = 7.04f;
            float maxX = 1.2f;
            float y = 29.06f;
            float minZ = -2.6f;
            float maxZ = -0.36f;


            // Generate random x, y, and z positions within the specified ranges
            float randomX = Random.Range(minX, maxX);
            float randomZ = Random.Range(minZ, maxZ);

            other.gameObject.transform.position = new Vector3(randomX, y, randomZ);
        }
    }

}
```

Ez alapvetően ellenőrzi, hogy megfelelő objektum kerül-e a dobozba (a Layer révén), ha igen akkor azt deaktiváljuk, és meghívjuk a **Level2Progress** SportItemPlaced függvényét. Ha nem megfelelő GameObject kerül bele, akkor pedig a szobába visszahelyezzük egy random helyre.

A **Level2Progress** szkript a következőképpen néz ki:

```C#
public void HatPlaced()
{
    hatPlaced++;
    if(hatPlaced == 3)
    {
        GetComponent<PlayQuickSound>().Play();
        subTasksCompleted++;
        if (subTasksCompleted == 3)
        {
            CheckIfLevelIsCompleted();
        }
        else
        {
            progressText.text = "Progress: " + subTasksCompleted + "/3";
            progress.GetComponent<FadeCanvas>().StartFadeIn();
            StartCoroutine(TurnOffClue(4f));
        }
       
    }
}

public void BookPlaced()
{
    bookPlaced++;
    if(bookPlaced == 3)
    {
        GetComponent<PlayQuickSound>().Play();
        subTasksCompleted++;
        if (subTasksCompleted == 3)
        {
            CheckIfLevelIsCompleted();
        }
        else
        {
            progressText.text = "Progress: " + subTasksCompleted + "/3";
            progress.GetComponent<FadeCanvas>().StartFadeIn();
            StartCoroutine(TurnOffClue(4f));
        }
       
    }
}

public void SportItemPlaced()
{
    sportItemPlaced++;
    if (sportItemPlaced == 6)
    {
        GetComponent<PlayQuickSound>().Play();
        subTasksCompleted++;
        if(subTasksCompleted == 3)
        {
            CheckIfLevelIsCompleted();
        }
        else
        {
            progressText.text = "Progress: " + subTasksCompleted + "/3";
            progress.GetComponent<FadeCanvas>().StartFadeIn();
            StartCoroutine(TurnOffClue(4f));
        }
       
    }
}

public void CheckIfLevelIsCompleted()
{
    if(subTasksCompleted == 3)
    {
        Door.SetActive(false);
    }
}

private IEnumerator TurnOffClue(float delay)
{
    // Wait for the specified delay
    yield return new WaitForSeconds(delay);
    progress.GetComponent<FadeCanvas>().StartFadeOut();

}
```

Alapvetően ezt tartja számon az egyes részfeladatok állapotát. Ha elérjük a megfelelő számú tárgyat, akkor kijelezzük ezt a játékosnak (hangeffektel is -> **PlayQuickSound**), és ha kész az összes részfeladat akkor az ajtót eltüntetjük.

Ezután a játékos áttérhet a következő pályára.

 ## Level 3

Itt is egy "hagyományos" szabadulószoba tárul a játékos elé. Itt a pálya Clue a következő: "Aim is the name of the game." 
Körbe kell néznie az egyes dinamikus objektumok között. A szobába alapvetően sötét van, ezért kézenfekvő, hogy van egy zseblámpa a szobában. A zseblámpa el van látva egy **ToggleLight** szkripttel:

```C#
//ToggleLight

public void Flip()
{
    isOn = !isOn;
    currentLight.enabled = isOn;
}
```

Ez egyszerűen a primary gomb hatására felkapcsolja / lekapcsolja a lámpát. Ezzel a lámpával a szobát megvizsgálva felfedezünk egy titkos üzenetet az egyik falon. Az üzenet túl kicsit, ezért a játékos felhaszálja a nagyítót ami a szobában van és így már el tudja olvasni a következő szöveget: "Looking things from lower may help you achieve the right perspective."
Ezzel utalva arra, hogy van egy fegyver az asztal alatt. Ez a fegyver tartalmazza a következő szkripteket:
- **PlayQuickSound** -> hogy a lővésnek legyen hangja (primary button)
- **Painting** -> itt újrafelhasználva, hogy alapvetően ne essen le amíg a játékos nem fogja meg
- **LaunchProjectile** szkript:

```C#
//LaunchProjectile

 public void Fire()
 {
     GameObject newObject = Instantiate(projectilePrefab, startPoint.position, startPoint.rotation);

     if (newObject.TryGetComponent(out Rigidbody rigidBody))
         ApplyForce(rigidBody);
 }

 private void ApplyForce(Rigidbody rigidBody)
 {
     Vector3 force = startPoint.forward * launchSpeed;
     rigidBody.AddForce(force);
 }
 ```

 Ez alapvetően a töltény GameObjektet lespawnolja a fegyver elé megfelelő erővel, így imitálva a lővést. A töltényeken van egy egyszerű **DestroyObject** szkript:

 ```C#
 //DestroyObject

 private void Start()
{
    Destroy(gameObject, lifeTime);
}
```

Ami annyit tesz, hogy 10 másodperc után despawnolja a töltényeket a teljesítmény növeléséért.

A fegyver ezután használható arra, hogy egy üvegfal mögé rejtett kart "szabaddá tegyünk". Az üveget meglőve meghívodik rajta a **BreakGlass** és a **PlayQuickSound** szkript:

```C#
//BreakGlass
private void OnTriggerEnter(Collider other)
{
    glass.GetComponent<PlayQuickSound>().Play();
    glass_1.SetActive(true);
    glass_2.SetActive(true);
    glass_3.SetActive(true);
    glass_4.SetActive(true);
    glass_5.SetActive(true);
    glass_6.SetActive(true);
    glass_7.SetActive(true);
    glass_8.SetActive(true);

    this.gameObject.SetActive(false);
}
```
Ez igazából az üvegszilánkokat lespawnolja egy üvegtörés hangeffekt mellett. 
Ezután a játékos már be tud nyúlni a karhoz, ezzel érintkezve meghívódik a **TurnOnLights** szkript a karon egy **PlayQuickSound** kiséretében:

```C#
//TurnOnLights

 private void OnTriggerEnter(Collider other)
 {
     this.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
     chandelier_1.GetComponentInChildren<Light>().enabled = true;
     chandelier_2.GetComponentInChildren<Light>().enabled = true;
     chandelier_3.GetComponentInChildren<Light>().enabled = true;
     chandelier_4.GetComponentInChildren<Light>().enabled = true;
     chandelier_5.GetComponentInChildren<Light>().enabled = true;
     chandelier_6.GetComponentInChildren<Light>().enabled = true;

     Door.SetActive(false);
 }
 ```
 Ez annyit csinál, hogy felkapcsolja az egyes lámpák Light komponenseit, illetve a kart lehúzott állapotba helyezi, valamint egy ajtót is megnyit.

 Ha az új szobába megy a játékos akkor egy céllövöldébe érkezik, ahol 3 célpontot kell eltalálni a fegyver segítségével. A célpontok random mozgásáért a **RandomMovement** szkript felel:
 
 ```C#
 //RandomMovement
 
  void Start()
 {
     // Initialize the first target position
     SetNewTargetPosition();
 }

 void Update()
 {
     // Move towards the target position
     transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

     // Check if the object has reached the target position
     if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
     {
         // Set a new target position
         SetNewTargetPosition();
     }
 }

 void SetNewTargetPosition()
 {
     float x = pointA.x;
     float y = Random.Range(pointB.y, pointA.y);
     float z = Random.Range(pointA.z, pointB.z);

     targetPosition = new Vector3(x, y, z);
 }

 private void OnTriggerEnter(Collider other)
 {
     LevelProgess.GetComponent<Level3Progress>().TargetHit();
     PlaySound.GetComponent<PlayQuickSound>().Play();
     this.gameObject.SetActive(false);
 }
```
Egy meghatározott területen belül random mozognak az egyes GameObjektumok.

 A célpontok eltalálásának kezelése is ebben a szkriptben történik az OnTriggerEnter függvényben, ez egy hangeffekt lejátszása mellett meghívja a **Level3Progress** szkript TargetHit függvényét:

 ```C#
 //Level3Progress

  public void TargetHit()
 {
     targetsHit++;
     if (targetsHit == 3)
     {
         Door.SetActive(false);
         GetComponent<PlayQuickSound>().Play();
         BoomBox.GetComponent<PlayContinuousSound>().Play();
     }
 }
 ```
 A 3 célpont eltalása után kinyílik a végső zöld ajtó, valamint egy kis háttérzene is megszólal.

 ## Level 4

Itt a játékosnak már csak néhány képet kell készítenie a szobában található kamera segítségével. A Clue: "Before you leave, please place your "memories" on the notebook."
3 képet kell készíteni és a fotóalbumra helyezni. A képek lehelyezéséért a **PhotoAdded** szkript felelős:

```C#
//PhotoAdded

 private void OnTriggerEnter(Collider other)
 {
     photoCount++;
     other.gameObject.SetActive(false);
     progressText.text = "Progress: " + photoCount + "/3";
     if (photoCount == 3)
     {
         Boombox.GetComponent<PlayContinuousSound>().PlayChampion();
         ending.GetComponent<FadeCanvas>().StartFadeIn();
     }
     else
     {
         progress.GetComponent<FadeCanvas>().StartFadeIn();
         StartCoroutine(TurnOffClue(4f));
     }
     
     
   
 }

 private IEnumerator TurnOffClue(float delay)
 {
     // Wait for the specified delay
     yield return new WaitForSeconds(delay);
     progress.GetComponent<FadeCanvas>().StartFadeOut();

 }
 ```
 Ez alapvetően annyit csinál, hogy számon tartja a lehelyezett képek számát, valamint ha megvan a 3 kép, akkor megjeleníti a gratuláló szöveget, illetve lejátszik egy győzedelmi zenét.

 A kamera képkészítése a **TakePhoto** szkript segítségével történik:

 ```C#
 //TakePhoto

public void Take_Photo()
{
    GameObject newObject = Instantiate(photoPrefab, startPoint.position, startPoint.rotation);
    newObject.transform.SetParent(startPoint);

    Rigidbody rb = newObject.GetComponent<Rigidbody>();
    if (rb != null)
    {
        rb.isKinematic = true; // Prevent falling
    }
    else
    {
        Debug.LogWarning("Rigidbody component not found on the prefab.");
    }

    Transform film = newObject.transform.Find("Photograph_Film");

    if (film != null)
    {
        MeshRenderer filmRenderer = film.GetComponent<MeshRenderer>();

        if (filmRenderer != null)
        {
            StartCoroutine(CapturePhoto(filmRenderer));
        }
        else
        {
            Debug.LogWarning("MeshRenderer component not found on the 'film' child object.");
        }
    }
    else
    {
        Debug.LogWarning("Child object 'film' not found.");
    }

    StartCoroutine(WaitForSecondsCoroutine(newObject));
}

private IEnumerator CapturePhoto(MeshRenderer filmRenderer)
{
    yield return new WaitForEndOfFrame();

    // Ensure the camera's clear flags and background color
    camera.clearFlags = CameraClearFlags.SolidColor;
    camera.backgroundColor = clearColor;

    RenderTexture currentRT = RenderTexture.active;
    RenderTexture.active = camera.targetTexture;

    camera.Render();

    // Capture the render image
    Texture2D image = new Texture2D(camera.targetTexture.width, camera.targetTexture.height, TextureFormat.RGB24, false);
    image.ReadPixels(new Rect(0, 0, camera.targetTexture.width, camera.targetTexture.height), 0, 0);
    image.Apply();
    RenderTexture.active = currentRT; // Reset active render texture

    filmRenderer.material.mainTexture = image;
}

private IEnumerator WaitForSecondsCoroutine(GameObject obj)
{
    // Wait for 5 seconds
    yield return new WaitForSeconds(5);

    // Code here will execute after 5 seconds
    obj.transform.SetParent(null);
    Rigidbody rb = obj.GetComponent<Rigidbody>();
    rb.isKinematic = false;
}
```

A kamerán van egy Camera GameObject és ennek a kirenderelt képét helyezzük egy film textúrájára. 5 másodperc után pedig engedélyezzük ennek a képnek a gravitációt (ezzel segítve hogy a felhasználó könnyedén le vegye a kinyomtatott képet a kamera elejéről).

 ## XR

 A XR input kezelése, illetve maga Main Camera (a játékos "szeme") ebben a komponensben van. Van egy bal és jobb oldali Ray, illetve egy Controller. A Ray az egy "nyaláb" amivel a felhasználó tud interaktálni az UI-al és teleportálni (ezt az egyes kontrollereken a primary gomb lenyomásával érjük el). Valamint a egy DirectInteractor ami úgy müködik mint egy valóságos "kéz", ezzel tudunk a dinamikus objektumokkal interaktálni.
