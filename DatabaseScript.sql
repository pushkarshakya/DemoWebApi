CREATE SCHEMA `demo_schema` ;
USE `demo_schema`;

DELIMITER $$
CREATE TABLE `object_type` (
  `object_type_id` int NOT NULL AUTO_INCREMENT,
  `object_type_name` varchar(45) DEFAULT NULL,
  `description` varchar(500) DEFAULT NULL,
  `level` int NOT NULL,
  PRIMARY KEY (`object_type_id`),
  CONSTRAINT `object_type_chk_1` CHECK ((`level` in (1,2,3,4,5)))
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `object_type_get_all`()
BEGIN
	SELECT * FROM object_type;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `object_type_delete`(IN in_object_type_id int)
BEGIN
	DELETE FROM object_type WHERE object_type_id=in_object_type_id;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `object_type_get_by_id`(IN in_object_type_id int)
BEGIN
	SELECT * FROM object_type WHERE object_type_id=in_object_type_id;
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `object_type_insert`(
	IN in_object_type_name varchar(45),
    IN in_description varchar(500),
    IN in_level int
)
BEGIN
	INSERT INTO object_type(object_type_name, description, level)
    VALUES(in_object_type_name, in_description, in_level);
END$$
DELIMITER ;

DELIMITER $$
CREATE DEFINER=`root`@`localhost` PROCEDURE `object_type_update`(
	IN in_object_type_id int,
	IN in_object_type_name varchar(45),
    IN in_description varchar(500),
    IN in_level int
)
BEGIN
	UPDATE object_type
    SET object_type_name=in_object_type_name, description=in_description, level=in_level
    WHERE object_type_id=in_object_type_id;
END$$
DELIMITER ;
