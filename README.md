# Dotnet FTP

This project was developed to test connection and file transfer using FTP

## Resources used

- DotNet 7
- FTP

## What is FTP?

FTP, or File Transfer Protocol, is a standard network protocol for exchanging files between a client and server on a TCP-based network like the internet. It operates on a client-server model, involving command and data channels. The client initiates a connection to the server to upload or download files. FTP has two modes: active (client opens a random port) and passive (server opens a port for data transfer). While fundamental for file exchange, FTP lacks encryption, leading to security concerns. Secure alternatives like FTPS and SFTP enhance FTP with encryption, ensuring safer and more confidential file transfers over the internet

<p align="start">
  <img src="./assets/ftp.png" width="160" />
</p>

## Test

To run this project you need docker installed on your machine, see the docker documentation [here](https://www.docker.com/).

Having all the resources installed, run the command in a terminal from the root folder of the project and wait some seconds to build project image and download the resources: `docker-compose up -d`

In terminal show this:

```console
 [+] Running 3/3
 ✔ Network dotnet-ftp_app_network  Created                        1.1s
 ✔ Container ftp                   Started                        16.4s
 ✔ Container ftp_app               Started                        1.6s
```

After this, access the link below:

- Swagger project [click here](http://localhost:5000/swagger)

### Stop Application

To stop, run: `docker-compose down`
