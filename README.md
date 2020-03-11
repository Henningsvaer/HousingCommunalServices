# Тестовое задание "ЖКХ"

## Документация
Решение состоит из двух проектов: **HousingCommunalServicesTestTask** и **HousingCommunalServicesClassLibrary**.

>______________________________________________

## HousingCommunalServicesTestTask
Основной проект, из которого происходит запуск приложения, также в его корневой директории лежит файл **Config.xml**.

>______________________________________________

## HousingCommunalServicesClassLibrary
Дополнительный проект, в котором размещен функционал приложения, разделение нужно чтобы этот модуль, в случае необходимости, мог переиспользоваться.
Включает в себя следующие компоненты: 

* **XML**
* **Report**
* **Manager**
* **GoogleAPI**

>______________________________________________

### XML

**User** >>> Структура для цельного представления данных, полученных из файла конфигурации.

**XMLReader** >>> Статический класс, который считывает информацию с помощью метода **Read** из файла конфигурации.

>______________________________________________

### Report

**IReport** >>> Интерфейс, который реализуют классы отвечающие за вывод информации.

**ConsoleReport** >>> Класс, отвечающий за вывод информации в консольном приложении.

>______________________________________________

### Manager
**HousingCommunalServicesManager** >>> Класс, отвечающий за получение информации от базы данных.

>______________________________________________

### GoogleAPI
**GoogleAPIAccountManager** >>> Класс, для взаимодействия с гугл аккаунтом.

>______________________________________________

## Начало использования.

* Создать бд postgresql

Приложение подключается к бд и поэтому нам нужно ее создать и скопировать информацию о ней в соответствующие поля снизу.

В проекте **HousingCommunalServicesTestTask**:

* Заполнить поля в **users_config.xml**
```
<dbconnection>
    <host>localhost</host>
    <username>****</username>
    <password>****</password>
    <database>****</database>
    <freediskspace>0.4</freediskspace>
    <range>Лист1!A1:D</range> <!--ИмяЛиста!столбец_от:столбец_до-->
</dbconnection>
  
```

* Заполнить поля в **credentials.json**
```
{
	"installed":
	{
		"client_id":"****",
		"project_id":"****",
		"auth_uri":"https://accounts.google.com/o/oauth2/auth",
		"token_uri":"https://oauth2.googleapis.com/token",
		"auth_provider_x509_cert_url":"https://www.googleapis.com/oauth2/v1/certs",
		"client_secret":"****",
		"redirect_uris":["urn:ietf:wg:oauth:2.0:oob","http://localhost"]
	}
}

```
