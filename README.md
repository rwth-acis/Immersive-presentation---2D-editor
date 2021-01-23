# ImPres
## An Immersive Presentation System

### System Functionalities
The Impres Systems allows users to create and present 3D presentations.
Those presentations can contain 2D content (presented on the so called "canvas") and 3D content.
There exist two types of 3D content.
One can be manipulated by all partitioners of the presentation (elements in the so called "handout").
And the other one that can not be manipulated during the live presentation (elements in the so called "scene").
Augmented Reality (AR) is used to present the 3D elements of the presentation.

### System Architecture
The System consist of 3 parts.

This repository contains the 2D Editor for the presentations.

The 3D editor part can be found in [this repository](https://github.com/rwth-acis/Immersive-presentation---3D-editor).

The backend that connects all parts can be found in [this repository](https://github.com/rwth-acis\Immersive-presentation---Backend-Coordinator).

### Getting started

First the backend should be up and running at a specified `<backend-addr>`.
Then the two editors (the (2D editor](https://github.com/rwth-acis/Immersive-presentation---2D-editor) and the [3D editor](https://github.com/rwth-acis/Immersive-presentation---3D-editor) can be initialized.)
As all architecture parts are needed, the getting started ssection of all architecture parts, each cover the same complete setup process for all parts.

#### Prerequisites
A MySQL Database which can be accessed by the backend
- MySQL database that uses the shema defined in the `<databasesetup.sql>` file in the [backend repository](https://github.com/rwth-acis\Immersive-presentation---Backend-Coordinator).
- User with the neccessary login credentials that can be used by the backend to connect to the database.

For the backend
- [Node.JS](https://nodejs.org/en/) installed (tested with version 14.15.4)
- A proxy pass from the `<backend-addr>` to a free port (here called `<backend-port>`)
- A folder where the presentation files can be stored in with a `<presfolder-path>`(the user that will execute the Node.js application needs read and write access to this folder)

For the 3D Editor
- Recommended [Unity version](https://unity3d.com/de/get-unity/download/archive): 2019.4.6f1
- [Microsoft Mixed Reality Toolkit v2.4.0](https://github.com/microsoft/MixedRealityToolkit-Unity/releases/tag/v2.4.0) (already included in the project)
- [Photon PUN 2](https://assetstore.unity.com/packages/tools/network/pun-2-free-119922) (download through Unity's asset store window in the editor)
- Visual Studio (tested with VS 2019)
- For HoloLens Development:
  - Windows 10 development machine
  - Windows 10 SDK ([10.0.18362.0](https://developer.microsoft.com/de-de/windows/downloads/windows-10-sdk))
- For Android Development:
  - [ARCore SDK](https://github.com/google-ar/arcore-unity-sdk/releases) (tested with ARCore SDK for Unity v1.12.0)
  - Android SDK 7.0 (API Level 24) or later

#### Backend

- Clone the [repository](https://github.com/rwth-acis/Immersive-presentation---Backend-Coordinator) to a server that fullfills all prerequisites.
- Create a .env file in the project folder and set the following Environment variables:

Environment Variable | Value or description
-------------------- | --------------------
PRODUCTION | if in production environment 1 else 0
PORT | `<backend-port>`
MYSQL_HOST | address where to reach the MySQL database
MYSQL_USER | name of the MySQL user
MYSQL_PASSWORD | password of the MySQL user
MYSQL_DATABASE | name of the MySQL database
JWT_SECRET | a secret string used to generate the JWT
SALTROUNDS | rounds of encrypton (e.g. 9)
PRES_DIR | `<presfolder-path>`
OIDCCLIENTID | [Learning Layers](http://results.learning-layers.eu/) client id for [OIDC](https://openid.net/connect/)
OIDCCLIENTSECRET | [Learning Layers](http://results.learning-layers.eu/) client secret for [OIDC](https://openid.net/connect/)