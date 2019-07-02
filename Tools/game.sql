/*
Navicat MySQL Data Transfer

Source Server         : Game
Source Server Version : 50717
Source Host           : localhost:3306
Source Database       : game

Target Server Type    : MYSQL
Target Server Version : 50717
File Encoding         : 65001

Date: 2019-06-22 16:27:11
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for 16_prop
-- ----------------------------
DROP TABLE IF EXISTS `16_prop`;
CREATE TABLE `16_prop` (
  `id` int(20) NOT NULL AUTO_INCREMENT,
  `propId` int(20) NOT NULL,
  `num` int(20) NOT NULL,
  `slot` int(20) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of 16_prop
-- ----------------------------
INSERT INTO `16_prop` VALUES ('1', '10011', '2', '0');
INSERT INTO `16_prop` VALUES ('2', '10012', '2', '1');
INSERT INTO `16_prop` VALUES ('3', '10013', '2', '0');

-- ----------------------------
-- Table structure for 17_prop
-- ----------------------------
DROP TABLE IF EXISTS `17_prop`;
CREATE TABLE `17_prop` (
  `id` int(20) NOT NULL AUTO_INCREMENT,
  `propId` int(20) NOT NULL,
  `num` int(20) NOT NULL,
  `slot` int(20) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of 17_prop
-- ----------------------------
INSERT INTO `17_prop` VALUES ('1', '10013', '2', '0');

-- ----------------------------
-- Table structure for 18_prop
-- ----------------------------
DROP TABLE IF EXISTS `18_prop`;
CREATE TABLE `18_prop` (
  `id` int(20) NOT NULL AUTO_INCREMENT,
  `propId` int(20) NOT NULL,
  `num` int(20) NOT NULL,
  `slot` int(20) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of 18_prop
-- ----------------------------
INSERT INTO `18_prop` VALUES ('4', '10016', '1', '1202');
INSERT INTO `18_prop` VALUES ('6', '10017', '1', '14');
INSERT INTO `18_prop` VALUES ('8', '10016', '1', '14');
INSERT INTO `18_prop` VALUES ('10', '10018', '1', '14');
INSERT INTO `18_prop` VALUES ('14', '10005', '1', '8');
INSERT INTO `18_prop` VALUES ('15', '10002', '6', '9');
INSERT INTO `18_prop` VALUES ('23', '10003', '7', '10');
INSERT INTO `18_prop` VALUES ('24', '10001', '3', '0');
INSERT INTO `18_prop` VALUES ('25', '10007', '1', '0');
INSERT INTO `18_prop` VALUES ('26', '10008', '1', '0');
INSERT INTO `18_prop` VALUES ('27', '10013', '1', '0');
INSERT INTO `18_prop` VALUES ('28', '10018', '1', '1');
INSERT INTO `18_prop` VALUES ('29', '10016', '1', '15');
INSERT INTO `18_prop` VALUES ('30', '10017', '1', '15');
INSERT INTO `18_prop` VALUES ('31', '10015', '1', '14');
INSERT INTO `18_prop` VALUES ('32', '10004', '2', '0');

-- ----------------------------
-- Table structure for role
-- ----------------------------
DROP TABLE IF EXISTS `role`;
CREATE TABLE `role` (
  `userId` int(50) NOT NULL AUTO_INCREMENT,
  `accountId` int(50) NOT NULL,
  `name` varchar(50) NOT NULL,
  `jobId` int(50) NOT NULL,
  `exp` int(255) NOT NULL,
  `level` int(50) NOT NULL DEFAULT '0',
  `localtion` varchar(255) NOT NULL DEFAULT '0,0,0',
  `signIn` varchar(255) NOT NULL DEFAULT '',
  `gold` int(50) NOT NULL COMMENT '金币',
  `bloodstore` int(50) NOT NULL COMMENT '血石',
  `gem` int(50) NOT NULL COMMENT '宝石',
  PRIMARY KEY (`userId`)
) ENGINE=InnoDB AUTO_INCREMENT=19 DEFAULT CHARSET=utf8 ROW_FORMAT=DYNAMIC;

-- ----------------------------
-- Records of role
-- ----------------------------
INSERT INTO `role` VALUES ('16', '10011', 'iu', '1003', '0', '0', '0,0,0', '|411 63695839961478|411 63695841128199|412 63695841128199|412 63695841128199|412 63695841128199|412 63695841128199|413 63695841128199|413 63695841128199|411 63695841178496', '0', '0', '0');
INSERT INTO `role` VALUES ('17', '10012', 'ss', '1003', '0', '0', '0,0,0', '|413 63696003935970', '0', '0', '0');
INSERT INTO `role` VALUES ('18', '10012', 'uuu', '1004', '0', '0', '0,0,0', '|413 63696008381728|414 63696073244345|417 63696354346949|418 63696422096691', '0', '100', '0');

-- ----------------------------
-- Table structure for test_prop
-- ----------------------------
DROP TABLE IF EXISTS `test_prop`;
CREATE TABLE `test_prop` (
  `id` int(20) NOT NULL AUTO_INCREMENT,
  `propId` int(20) NOT NULL,
  `num` int(20) NOT NULL,
  `slot` int(20) NOT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of test_prop
-- ----------------------------

-- ----------------------------
-- Table structure for usermsgtable
-- ----------------------------
DROP TABLE IF EXISTS `usermsgtable`;
CREATE TABLE `usermsgtable` (
  `accountId` int(11) NOT NULL AUTO_INCREMENT,
  `username` varchar(50) NOT NULL,
  `password` varchar(50) NOT NULL,
  `deviceID` varchar(50) DEFAULT NULL,
  `channel` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`accountId`)
) ENGINE=InnoDB AUTO_INCREMENT=10013 DEFAULT CHARSET=utf8;

-- ----------------------------
-- Records of usermsgtable
-- ----------------------------
INSERT INTO `usermsgtable` VALUES ('10011', 'io', '123', 'b03b60169aa5fee8b7503771bad66521b0083593', 'UC');
INSERT INTO `usermsgtable` VALUES ('10012', 'ppp', '123', 'b03b60169aa5fee8b7503771bad66521b0083593', 'UC');
