# Garmin Toolbox .net

All in all its a bunch of hacked code while having the child in one hand and the keyboard in the other.
The target is to have some tools around Garmin Connect (<https://connect.garmin.com/)> to download metadata, .fit and .gpx files for my activities and to further sort/copy them to have a file base for visualization.

- .net core 2.1

Having it running once a day (via cron) on a linux laptop. Works perfectly ;-)

## Garmin Connect Exporter

- login into garmin connect
- download last 50 activities metadata
- store in mysql
- download the original files for the activities
- download gpx files for the activities (if available)
- disconnect
- send a mail with the activities of the last 7 days (bugs over bugs in this one)

## GpxSorter

- get all metadata from the database
- copy all matching gpx files in a folder structure separated by year and month

Yes, I Know, there are hard coded credentials in this part. But you will not be able to do any harm with them ;-)

# Setup

## MySql

```
CREATE TABLE IF NOT EXISTS `activity_metadata` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `activity_id` varchar(16) NOT NULL,
  `name` varchar(255) NOT NULL,
  `activity_type` varchar(255) NOT NULL,
  `begin` datetime DEFAULT NULL,
  `end` datetime DEFAULT NULL,
  `distance` double DEFAULT NULL,
  `begin_longitude` double DEFAULT NULL,
  `begin_latitude` double DEFAULT NULL,
  `end_longitude` double DEFAULT NULL,
  `end_latitude` double DEFAULT NULL,
  `has_original` tinyint(1) DEFAULT '0',
  `has_gpx` tinyint(1) DEFAULT '0',
  `gpx_download_failed` tinyint(1) NOT NULL DEFAULT '0',
  `duration` double DEFAULT NULL,
  `movingDuration` double DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=5901 DEFAULT CHARSET=utf8;
```

## Settings

The following needs to go in a json file beeing passed as a parameter to GarminConnectExporter as its single argument.

```
{
  "DbSettings": {
    "Server": "db host",
    "Database": "database name",
    "Username": "username",
    "Password": "pwd"
  },
  "GarminSettings": {
    "Username": "username",
    "Password": "pwd"
  },
  "MailSettings": {
    "SmtpServer": "smtp host",
    "SmtpServerPort": port,
    "SmtpUser": "user",
    "SmtpPassword": "pwd",
    "Sender": "sender mail",
    "Recipient": "recipient mail",
    "Subject": "Garmin Export - Report"
  },
  "FileSystem": {
    "OriginalFileDownloadDirectory": "path",
    "GpxFileDownloadDirectory": "path"
  }
}
```

# Goals

- refac ;-) & some test
- directly draw gpx files to a jpg/png
- correct all bugs in the reporting module
- ability to redownload certain date ranges (currently missing large sections of 2018 in my data)
- ...
