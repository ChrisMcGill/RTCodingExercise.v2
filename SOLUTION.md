# Solution: Audit History Feature

The goal of this task was to track the history of every number plate. Whenever a plate is created, sold, reserved, updated or deleted we need to save a record of what change happened and when for audit purposes. Users (employees, not customers) also need to be able to view this history and download it

## My Approach

Backend
I decided to create a separate table for the history instead of adding more columns to the existing Plates table. This keeps the data organised and prevents the main table from getting overloaded with information
* I created an `AuditLog` class with an ID, the action name (like "Sold"), a description, and a timestamp
* I built an `AuditRepository` to handle saving and retrieving these logs
* I updated the existing `PlateRepository` so that whenever it saves a plate, it automatically calls the Audit Repository to save a log entry at the same time

API
I added a new Controller (`AuditController`) with two endpoints:
1.  Get History: Fetches the list of changes for a specific plate
2.  Export: Generates a JSON file of that history for the user to download

Frontend (Angular)
I updated the Plate List component:
* Added a "View History" button to each card
* When clicked, it asks the API for the history
* It displays the history in a popup message
* It also gives the user an option to download the data as a JSON file

Testing
I wrote Unit Tests for the new features.
* Instead of setting up a complex real database for testing, I used a test repository" (a List in memory)
* This allowed me to test the Controller logic quickly without needing to install extra tools or mock complex database connections
