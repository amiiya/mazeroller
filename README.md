# Maze Roller

## Arduino_Script
Um das Skript auf das Gerät zu laden, wird die Arduino Software benötigt. Über den Reiter "Werkzeuge" müssen folgende Einstellungen gesetzt werden:
* Board: "Arduino Nano"
* Prozessor: "ATmega328P (Old Bootloader)"
* Port: COM? (Port von dem angeschlossenen Gerät)

In der Ausgabe müssen dann lesbare Daten ausgegeben werden.

## Labyrinth
Bevor das Spiel in Unity gestartet werden kann, muss das das .ino-Skript auf das Gerät geladen werden. Dieser Schritt muss jedes Mal ausgeführt werden, wenn das Gerät getrennt wird.

Beim Board muss ebenfalls der entsprechende Port eingetragen werden. Bei einem falschen Port sollte Unity Fehlermeldungen ausgeben, die daraufhinweisen.

## Python
Der implementierte Filter bei der als final gekennzeichneten Datei eingesehen werden. Dies wird durch Grafiken veranschaulicht. Hierfür wurden Daten vom dem Gerät aufgenommen und in eine .csv abgespeichert, die mit Python anschließend gefiltert werden. Hierfür muss das Gerät nicht angeschlossen sein.
