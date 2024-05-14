# ScriptAdventure
## Descrizione

Questo progetto è stato realizzato come progetto di laurea triennale in Scienze e Tecnologie Informatiche presso l'Università degli Studi della Basilicata. L'obbiettivo era quello di creare un videogioco OpenSource che 
potesse avvicinare gli studenti al mondo della programmazione. Il gioco è stato sviluppato con l'engine Godot in C#, scaricando ed importando il progetto da questa pagina sarà possibile modificare liberamente il progetto e aggiungere funzionalità.
All'interno del progetto è presente un file di Documentazione che spiega come importare il progetto e come funzionano le varie componenti dell'applicazione.
Il gioco contiene 2 modalità: 
  1)Modalità esercizio: Utile per i professori, permette di sottoporre agli studenti degli esercizi in cui, attraverso l'interfaccia grafica, dovranno completare degli Script in modo corretto.
  2)Modalità campagna: Gli studenti possono divertirsi ad esplorare un mondo isometrico in 2D diviso in livelli, in cui dovranno usare le loro doti da programmatori per interagire con il mondo e risolvere i vari puzzle.

## Modalità Esercizio

I professori potranno impostare le sessioni di esercizi attraverso una pagina di configurazione interna all'applicazione selezionando le varie opzioni come: Argomenti, livello di difficoltà, numero di esercizi per categoria, ecc...
Sarà anche possibile configurare il tutto attraverso un file esterno che verrà creato automanticamente al percorso "Documents\ScriptAdventure\", in questo modo all'avvio il gioco verrà lanciato già con la modalità esercizio avviata e configurata.
Alla fine della sessione di esercizi sarà possibile visionare una pagina di riepilogo che mostrerà il tempo impiegato e numero di errori per categoria, in modo da capire in quali argomenti sono presenti più lacune. 
Importando il progetto sarà possibile creare nuovi esercizi o aggiungere nuove categorie, per capire come fare basta visionare la documentazione.

## Modalità Campagna

La meccanica unica della modalità campagna è la ScriptConsole, ovvero un finto compilatore realizzato per riconoscere istruzioni precise, individuare i parametri necessari ed eseguire metodi precedentemente preparati capaci di eseguire tali funzionalità.
In questo modo si da all'utente l'impressione che il codice scritto venga compilato ed eseguito sul momento, evitando che del codice sintatticamente o semanticamente sbagliato possa causare situazioni inattese e/o frustranti per il giocatore.
Il giocatore controllerà un personaggio realizzato attraverso una spriteSheet all'interno di un mondo 2D con visuale isometrica, avvicinandosi a degli oggetti interagibili potrà selezionarli e successivamente aprire la ScriptConsole per interagirci.
A questo punto potrà aprire anche una pagina di documentazione attraverso la quale potrà visionare parametri e metodi dell'oggetto selezionato, in modo da capire come interagirci attraverso il codice, e potrà inoltre visionare le funzionalità del linguaggio di 
programmazione implementato nella ScriptConsole.
Sono presenti attualmente solo 2 livelli demo che introducono il giocatore alle meccaniche e mostrano l'incipit della storia, all'interno del progetto è però presente tutto il materiale necessario per creare nuovi livelli, aggiungere funzionalità alla ScriptConsole 
e creare nuovi oggetti interagibili. Per capire come fare basta fare riferimento alla documentazione.

## BrokerMQtt

All'interno del progetto è stata inclusa anche una libreria per comunicare con BrokerMqtt, in questo modo è possibile iniviare messaggi al broker in condizioni specifiche. In questo modo gli studenti potrebbero voler creare degli script esterni, ad esempio su 
piattaforma Arduino, per comunicare con il gioco. Per poter fare ciò è stata creata una classe statica e una interfaccia da implementare per poter facilmente gestire la connessione e la comunicazione con il broker.

## Dettagli sul Progetto e Sviluppi Futuri

Il progetto è stato realizzato in circa 8 mesi mentre ultimavo gli ultimi esami, per ulteriori dettagli su come è stato sviluppato il tutto è possibile visionare il pdf della mia tesi in allegato a questo progetto.
Attualmente questo progetto è sicuramente ancora incompleto e potrebbe essere migliorato sotto molti aspetti, tuttavia non avrò probabilmente tempo per continuare a svilupparlo. 

## Crediti e risorse esterne

All'interno del progetto io(Venafro Daniele) mi sono occupato direttamente di tutto ciò che riguardasse elementi grafici, la programmazione in generale e il design/progettazione delle varie meccaniche di gioco, 
il tutto sotto la supervisione del Professore Michele Santomauro dell'Università degli Studi della Basilicata.

Di seguito gli Addon/Librerie esterne utilizzate nel progetto.
  1) More Effective Godot Coroutines (C#): https://github.com/WeaverDev/More-Effective-Godot-Coroutines?tab=readme-ov-file 
     - Copyright (c) 2023 Teal Rogers, Isar Arason (WeaverDev)
  2) PlayerPrefsUtility (C#): https://github.com/DrOffensive/PlayerPrefsUtility?tab=readme-ov-file
     - Copyright (c) 2023 MarcWerk
  3) hivemq-mqtt-client-dotnet: https://github.com/hivemq/hivemq-mqtt-client-dotnet?tab=Apache-2.0-1-ov-file              
     - Copyright 2022 HiveMQ GmbH   
