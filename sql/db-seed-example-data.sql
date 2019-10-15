/*!40000 ALTER TABLE `mqtt_users` DISABLE KEYS */;
INSERT INTO `mqtt_users` (`Id`, `Username`, `Password`, `LastHandshakeAt`) VALUES
	(1, 'admin', '$2y$12$UclqSARIcHofSjJJWTYncOegzQILwxDPNP6E7bhmONVsmpug8BDGu', '2019-10-15 17:30:00'), /* Pass: 123*/
	(2, 'user', '$2y$12$sEMIs3muc4IRY.fHPFCSSOPAtXq.qrffrtmxeX7pdenxNWclXsWeG', '2019-10-15 18:00:00'); /* Pass: 456 */
/*!40000 ALTER TABLE `mqtt_users` ENABLE KEYS */;

/*!40000 ALTER TABLE `mqtt_user_acl` DISABLE KEYS */;
INSERT INTO `mqtt_user_acl` (`Id`, `MqttUserId`, `Type`, `TopicPattern`) VALUES
	(3, 1, 'pub', '#'),
	(4, 1, 'sub', '#'),
	(5, 2, 'pub', 'sensors/+/metrics/#'),
	(6, 2, 'pub', 'sensors/+/actions/restart'),
	(7, 2, 'sub', 'sensors/1/metrics/#');
/*!40000 ALTER TABLE `mqtt_user_acl` ENABLE KEYS */;

/*!40000 ALTER TABLE `mqtt_user_handshakes` DISABLE KEYS */;
INSERT INTO `mqtt_user_handshakes` (`Id`, `MqttUserId`, `HandshakeAt`) VALUES
	(1, 1, '2019-10-15 17:00:00'),
	(2, 1, '2019-10-15 17:30:00'),
	(3, 2, '2019-10-15 18:00:00');
/*!40000 ALTER TABLE `mqtt_user_handshakes` ENABLE KEYS */;