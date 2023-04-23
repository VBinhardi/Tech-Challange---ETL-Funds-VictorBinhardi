### Tech-Challange---ETL-Funds-VictorBinhardi
**ETL CVM FUNDS (JobInterview) - Victor Binhardi Corrêa**


### WebAPI Features

This is a *C#.NET Web API* application that runs on .NET 7 and has the purpose to download daily CVM fund data from their website, populate all the records into the database and making them available for consultation with specific filters.
<br/>
<br/>
It consists of only 1 controller with 2 endpoints **FetchDataAndLoadDatabase** and **GetRecords**.


# Startup

The first thing that the program does when it is run is create the database table structure that is going to be used by rest of the application.
Whenever it is run again it verifies if the table already exists and only runs the creation script if it doesn't.

# FetchDataAndLoadDatabase

<summary>

First, it builds a list of the url's of all the files it needs to download from 2017 until today based on today's date, so it **will continue to work as time passes on**

</summary>
<summary> 

If it doesn't already exists, it creates a folder in the application root called **DownloadedZipFile** 
</summary>

<summary>

Using **asynchronous programming**, it starts to download all the files **simultaneously**

</summary>

<summary>

After all the tasks finish downloading the files, the applicaton creates another subfolder called **ExtractedCsvFiles**

</summary>

<summary>

Once again, using **asynchronous programming**, it starts do extract all files **at the same time** to the subfolder.

</summary>

<summary>

At last, the endpoint reads one file at a time for reduced memory consumption, builds a **single** insert SQL Command for **better performance** and run it to insert the records into the database. For me this process took around **7 ~ 8 minutes** to insert all the **28 millions** records into the database, but could vary depending on the computer.

</summary>

# GetRecords

<summary>First, it ensures that the mandatory parameter is valid and present.</summary>
<summary>

Based on the ammount of optional parameters, it begins to build an SQL Command String and inject it with the parameters, which also protects the code against **SQL Injection**

</summary>
<summary>

The application runs the SQL Command and retrieves all the records filtered by the parameters and **ordered by date**, returning it to the user

</summary>

# Setup

The first thin you will need to alter is the connection string in the BLL project's web.config file.</br>
There you can also alter the url path for downloading the files from the CVM website.</br>
</br>
The endpoint's call path will be displayed below in two ways.

# Unit Tests
As *FetchDataAndLoadDatabase* endpoint has primarily has I/O and file systems in all of its steps, is not covered by unit tests as these items are considered to be infrastructure, which are only evaluated by integration tests.
</br>
For *GetRecords* endpoint, the unit tests only cover the build-up of the SQL Command Strings, as interactions with database are also only evaluated in integration tests.

# Calling the application endpoints

For quickly testing the application endpoint and passing the parameters you have two options:
<summary>When the application is run it is configured to run Swagger UI, so it is easier to test. The format for the parameters are as below</summary>

```
CNPJ [String] - Mandatory Parameter - Example: "00.068.305/0001-35"  (Without quotation marks)
StartDate [DateTime('MM-DD-YYYY')] - Optional Parameter - Example: "11-01-2017" (Without quotation marks)
EndDate [DateTime('MM-DD-YYYY')] - Optional Parameter - Example: "11-01-2017" (Without quotation marks)
```
<summary>You can use PostMan, below is the json collection export:</summary></br>

```json

{
	"info": {
		"_postman_id": "ea300e3c-246f-426e-ad64-304d8b5fb425",
		"name": "ETL FUND",
		"schema": "https://schema.getpostman.com/json/collection/v2.1.0/collection.json"
	},
	"item": [
		{
			"name": "FetchDataAndLoadDatabase",
			"request": {
				"method": "GET",
				"header": [],
				"url": {
					"raw": "https://localhost:44311/api/CVMFunds/FetchDataAndLoadDatabase",
					"protocol": "https",
					"host": [
						"localhost"
					],
					"port": "44311",
					"path": [
						"api",
						"CVMFunds",
						"FetchDataAndLoadDatabase"
					]
				}
			},
			"response": []
		},
		{
			"name": "GetRecords",
			"request": {
				"method": "GET",
				"header": [],
				"url": {
					"raw": "https://localhost:44311/api/CVMFunds/FetchDataAndLoadDatabase",
					"protocol": "https",
					"host": [
						"localhost"
					],
					"port": "44311",
					"path": [
						"api",
						"CVMFunds",
						"FetchDataAndLoadDatabase"
					]
				}
			},
			"response": []
		}
	]
}

```
