# RFID_OPCUA_Service

## Установка службы Windows
Порядок установки
1. Выполнить в PowerShell
  > New-Service -Name <Имя службы> -BinaryPathName "<Полный каталог до .exe файла>.exe" 
2. Запустить службу
  > (Get-Service <Имя службы>).Start()

## Отправка команд службе
  > (Get-Service <Имя службы>).ExecuteCommand(<Номер команды>)

## Список команд поддерживаемых службой
  - Начать сканирование : 130
  - Остановить сканирование : 131
  - Считать область памяти : 140
  - Записать в область памяти : 141

## Параметры реестра создаваемые службой
  ### Общая структура
    HKEY_CURRENT_USER\Software\RFID - Корневой элемент в иерархии
    HKEY_CURRENT_USER\Software\RFID\args - Раздел для конфигурирования общих команд для всех считывателей
    HKEY_CURRENT_USER\Software\RFID\config - Параметры необходимые для иницализации службы
    HKEY_CURRENT_USER\Software\RFID\List - Раздел для хранения экземплярных данных
  ### Раздел args
    numRFID - номер считывателя на который необходимо отправить команду, сопоставляется с элементами из списков config
    numRP - номер антенны (ReadPoint), может принимать значения от 0 до 4
  ### Раздел config
    ipPort - список ip адресов считывателя в формате <ip>:<port>
    name - список произвольных имен для считывателей (ДОЛЖНЫ БЫТЬ УНИКАЛЬНЫМИ)
  ### Раздел List
    Содержит разделы с именем экземпляра считывателя
    каждый раздел содержит подразелы args и result
    args - содержит в себе параметры для выполнения функций
    result - содержит результаты выполненной функции на считывателе
