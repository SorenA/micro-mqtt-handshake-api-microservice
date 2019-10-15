CREATE TABLE IF NOT EXISTS `mqtt_users` (
  `Id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `Identity` varchar(128) NOT NULL,
  `Username` varchar(128) NOT NULL,
  `Password` varchar(512) NOT NULL,
  `LastHandshakeAt` timestamp NULL DEFAULT NULL,
  PRIMARY KEY (`Id`),
  UNIQUE KEY `Username` (`Username`),
  UNIQUE KEY `Identity` (`Identity`)
) ENGINE=InnoDB AUTO_INCREMENT=10 DEFAULT CHARSET=utf8mb4;

CREATE TABLE IF NOT EXISTS `mqtt_user_acl` (
  `Id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `MqttUserId` bigint(20) unsigned NOT NULL,
  `Type` varchar(64) NOT NULL,
  `TopicPattern` varchar(512) NOT NULL,
  PRIMARY KEY (`Id`),
  KEY `FK_MqttUserId` (`MqttUserId`),
  CONSTRAINT `FK_MqttUserId` FOREIGN KEY (`MqttUserId`) REFERENCES `mqtt_users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=12 DEFAULT CHARSET=utf8mb4;

CREATE TABLE IF NOT EXISTS `mqtt_user_handshakes` (
  `Id` bigint(20) unsigned NOT NULL AUTO_INCREMENT,
  `MqttUserId` bigint(20) unsigned NOT NULL,
  `HandshakeAt` timestamp NOT NULL DEFAULT current_timestamp() ON UPDATE current_timestamp(),
  PRIMARY KEY (`Id`),
  KEY `FK_Handshake_MqttUserId` (`MqttUserId`),
  CONSTRAINT `FK_Handshake_MqttUserId` FOREIGN KEY (`MqttUserId`) REFERENCES `mqtt_users` (`Id`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8mb4;