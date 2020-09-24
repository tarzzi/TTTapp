SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION';

CREATE SCHEMA IF NOT EXISTS `N4378_3` DEFAULT CHARACTER SET latin1 ;
USE `N4378_3` ;

-- -----------------------------------------------------
-- Table `N4378_3`.`CLIENT`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `N4378_3`.`CLIENT` (
  `clientID` MEDIUMINT(9) NOT NULL AUTO_INCREMENT,
  `fname` VARCHAR(30) NOT NULL,
  `lname` VARCHAR(45) NOT NULL,
  `address` VARCHAR(45) NOT NULL,
  `city` VARCHAR(45) NOT NULL,
  `postal` VARCHAR(10) NULL DEFAULT NULL,
  `phone` VARCHAR(15) NULL DEFAULT NULL,
  `email` VARCHAR(40) NOT NULL,
  PRIMARY KEY (`clientID`),
  UNIQUE INDEX `clientID_UNIQUE` (`clientID` ASC))
ENGINE = InnoDB
AUTO_INCREMENT = 29
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `N4378_3`.`ITEM`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `N4378_3`.`ITEM` (
  `itemID` INT(11) NOT NULL AUTO_INCREMENT,
  `itemName` VARCHAR(45) NOT NULL,
  `price` FLOAT NOT NULL,
  `category` VARCHAR(45) NOT NULL,
  `stockCount` MEDIUMINT(9) NOT NULL,
  `url` VARCHAR(50) NULL DEFAULT NULL,
  PRIMARY KEY (`itemID`),
  UNIQUE INDEX `tuoteID_UNIQUE` (`itemID` ASC))
ENGINE = InnoDB
AUTO_INCREMENT = 60
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `N4378_3`.`PET`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `N4378_3`.`PET` (
  `petID` SMALLINT(6) NOT NULL AUTO_INCREMENT,
  `name` VARCHAR(45) NOT NULL,
  `age` TINYINT(4) NULL DEFAULT NULL,
  `colour` VARCHAR(45) NULL DEFAULT NULL,
  `gender` VARCHAR(20) NULL DEFAULT NULL,
  `url` VARCHAR(50) NULL DEFAULT NULL,
  `breed` VARCHAR(20) NULL DEFAULT NULL,
  PRIMARY KEY (`petID`),
  UNIQUE INDEX `petID_UNIQUE` (`petID` ASC))
ENGINE = InnoDB
AUTO_INCREMENT = 22
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `N4378_3`.`PURCHASE`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `N4378_3`.`PURCHASE` (
  `purchaseID` INT(11) NOT NULL AUTO_INCREMENT,
  `CLIENT_clientID` MEDIUMINT(9) NOT NULL,
  PRIMARY KEY (`purchaseID`),
  UNIQUE INDEX `purchaseID_UNIQUE` (`purchaseID` ASC),
  INDEX `fk_PURCHASE_CLIENT1_idx` (`CLIENT_clientID` ASC))
ENGINE = InnoDB
AUTO_INCREMENT = 25
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `N4378_3`.`PURCHASE_has_ITEM`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `N4378_3`.`PURCHASE_has_ITEM` (
  `PURCHASE_purchaseID` INT(11) NOT NULL,
  `ITEM_itemID` INT(11) NOT NULL,
  PRIMARY KEY (`PURCHASE_purchaseID`, `ITEM_itemID`),
  INDEX `fk_PURCHASE_has_ITEM_ITEM1_idx` (`ITEM_itemID` ASC),
  INDEX `fk_PURCHASE_has_ITEM_PURCHASE1_idx` (`PURCHASE_purchaseID` ASC),
  CONSTRAINT `fk_PURCHASE_has_ITEM_PURCHASE1`
    FOREIGN KEY (`PURCHASE_purchaseID`)
    REFERENCES `N4378_3`.`PURCHASE` (`purchaseID`)
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_PURCHASE_has_ITEM_ITEM1`
    FOREIGN KEY (`ITEM_itemID`)
    REFERENCES `N4378_3`.`ITEM` (`itemID`)
    ON DELETE NO ACTION
    ON UPDATE NO ACTION)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;
