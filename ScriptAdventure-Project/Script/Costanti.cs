using System;

public class Costanti
{
    /// //////////////////////Input Action///////////////////////
    
    public static string MoveUp = "Move_up";
    public static string MoveDown = "Move_down";
    public static string MoveLeft = "Move_left";
    public static string MoveRight = "Move_right";
    public static string Interact = "Interact";
    public static string OpenConsole = "OpenConsole";
    public static string LogCodeUp = "LogCodeUp";
    public static string LogCodeDown = "LogCodeDown";
    public static string OpenDock = "OpenDock";
    public static string OpenMenu = "OpenMenu";

    ///////////////////////Direction///////////////////////////
    
    public static string DirectionUp = "up";
    public static string DirectionLeft = "left";
    public static string DirectionLeftUp = "left-up";
    public static string DirectionLeftDown = "left-down";
    public static string DirectionRight = "right";
    public static string DirectionRightUp = "right-up";
    public static string DirectionRightDown = "right-down";
    public static string DirectionDown = "down";

    //////////////////////Tag Coroutine///////////////////////////
    
    public static string PlayerAnimationTag = "player-anim";

    //////////////////////Type///////////////////////////
    
    public static string TypeString = "string";
    public static string TypeInt = "int";
    public static string TypeFloat = "float";

    //////////////////////Output Console///////////////////////////

    public static string ErrorNotRecognizedCode = "***ERROR*** No input code recognized";
    public static string ErrorNotvalidParameters = "***Error*** No valid expression in Parameters";
    public static string ErrorInfiniteCycle = "***Error*** Infinite cycle defined";
    public static string ErrorOutOfBounds = "***Error*** Out of Bounds";
    public static string ErrorNoValidType = "***Error*** No valid type recognized";
    public static string ErrorPropertyNotExist = "***Error*** The selected property does not exists";
    public static string Error = "***Error*** ";
    public static string ErrorException = "Error Exception: ";
    public static string WarningCycleNotStart = "***Warning*** cycle don't start";
    public static string WarningParameterNotFound = "***Warning*** parameter not found";

    ////////////////////////GroupName////////////////////////////////////////////////
    
    public static string UniqueNameGroup = "ScriptUniqueName";

    ////////////////////////UI Example elements////////////////////////////////////////////////
    
    public static string PropertiesDoc = "PropertiesDoc";
    public static string MethodsDoc = "MethodsDoc";
    public static string PropertiesVBoxContainer = "VBoxContainer";
    public static string PropertiesValueVBoxContainer = "VBoxContainer2";
    public static string PropertiesDescVBoxContainer = "VBoxContainer3";
    public static string MethodsVBoxContainer = "VBoxContainer";
    public static string MethodsDescVBoxContainer = "VBoxContainer2";
    public static string ExampleProperty = "ExampleProperty";
    public static string ExamplePropertyValue = "ExamplePropertyValue";
    public static string ExamplePropertyDesc = "ExamplePropertyDesc";
    public static string ExampleMethod = "WaitMethod";
    public static string ExampleMethodDesc = "WaitDescription";

    ////////////////////////Property Name////////////////////////////////////////////////

    public static string PropertyFrame = "Frame";
    public static string PropertyTestBool = "TestBool";
    public static string PropertyRegisteredObj = "RegisteredObj";
    public static string PropertyUniqueName = "UniqueName";

    ////////////////////////Property Description////////////////////////////////////////////////
    
    public static string PropertyFrameDesc = "Valore intero che indica il frame di una animazione.";
    public static string PropertyTestBoolDesc = "Valore booleano di prova per testare funzionalità nell'alpha.";
    public static string PropertyRegisteredObjDesc = "Stringa che rappresenta lo uniqueName(scrivilo senza le virgolette) dell'oggetto collegato al metodo Action dell'oggetto selezionato";
    public static string PropertyUniqueNameDesc = "Stringa che rappresenta il nome unico di un oggetto per poterlo riferire nel codice eseguito da un altro oggetto di gioco";

    ////////////////////////Method Name////////////////////////////////////////////////
    
    public static string MethodAction = "Action();";
    //public static string MethodMove = "Move(int id, int x, int y);";
    public static string MethodMoveUp = "MoveUp(int id);";
    public static string MethodMoveDown = "MoveDown(int id);";
    public static string MethodMoveLeft = "MoveLeft(int id);";
    public static string MethodMoveRight = "MoveRight(int id);";

    ////////////////////////Property Description////////////////////////////////////////////////
    
    public static string MethodActionDesc = "Metodo eseguito quando si interagisce con l'oggetto selezionato.";
    //public static string MethodMoveDesc = "Muovi l'oggetto con il numero id, lungo l'asse X e Y sulla scacchiera.";
    public static string MethodMoveUpDesc = "Muovi l'oggetto con il numero id in alto lungo la scacchiera.";
    public static string MethodMoveDownDesc = "Muovi l'oggetto con il numero id in basso lungo la scacchiera.";
    public static string MethodMoveLeftDesc = "Muovi l'oggetto con il numero id a sinistra lungo la scacchiera.";
    public static string MethodMoveRightDesc = "Muovi l'oggetto con il numero id a destra lungo la scacchiera.";

    ////////////////////////ExerciseMode////////////////////////////////////////////////

    public static string SubjectDeclaration = "Dichiarazione Variabili";
    public static string SubjectIf = "If";
    public static string SubjectFor = "Ciclo for";
    public static string SubjectWhile = "Ciclo while";
    public static string SubjectArray = "Array";
    public static string SubjectStruct = "Struct";
    public static string BaseLevel = "Base";
    public static string AdvancedLevel = "Avanzato";
    public static string If1ArgSpawnerDef = " if(...){}";
    public static string If3ArgsSpawnerDef = " if(... ... ...){}";
    public static string ForIntiSpawnerDef = " for(int i){}";
    public static string ForIntjSpawnerDef = " for(int j){}";
    public static string ElseSpawnerDef = " else{}";
    public static string While3ArgsSpawnerDef = " while(... ... ...){}";
    public static string While1ArgSpawnerDef = " while(...){}";

    ////////////////////////Options////////////////////////////////////////////////
    
    public static string windowed = " Finestra ";
	public static string windowedNoBorder = " Finestra senza bordi ";
	public static string fullscreen = " Schermo intero ";

    ////////////////////////PlayerPrefs////////////////////////////////////////////////
    
    public static string keyDisplay = "DisplaySelector";
	public static string keyResolution = "Resolution";
	public static string ipBroker = "ipBroker";
	public static string portBroker = "portBroker";
	public static string usernameBroker = "usernameBroker";
	public static string passwordBroker = "passwordBroker";
	public static string isBrokerConnectionOn = "isBrokerConnectionOn";
	public static string musicValue = "musicValue";
	public static string soundValue = "soundValue";
	public static string level0 = "level0";
	public static string level1 = "level1";

    ////////////////////////Broker////////////////////////////////////////////////

    public static string brokerTopicExerciseMode = "ExerciseMode";
    public static string brokerTopicCampaignMode = "CampaignMode";
    public static string brokerMessageSuccess = "Success";
    public static string brokerMessageError = "Error";
    public static string brokerMessageBadSuccess = "BadSuccess";
	
    ////////////////////////Audio Bus////////////////////////////////////////////////
    
    public static string AudioBusMusic = "Music";
    public static string AudioBusSound = "Se";
}
