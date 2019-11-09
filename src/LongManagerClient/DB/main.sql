/*
Navicat SQLite Data Transfer

Source Server         : LongClient
Source Server Version : 30802
Source Host           : :0

Target Server Type    : SQLite
Target Server Version : 30802
File Encoding         : 65001

Date: 2019-11-09 15:56:37
*/

PRAGMA foreign_keys = OFF;

-- ----------------------------
-- Table structure for BLSOutInfo
-- ----------------------------
DROP TABLE IF EXISTS "main"."BLSOutInfo";
CREATE TABLE "BLSOutInfo" (
"ID"  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
"RowGuid"  TEXT,
"MailNO"  INTEGER,
"AddDate"  TEXT,
"PostDate"  TEXT,
"OrgName"  TEXT,
"Consignee"  TEXT,
"Address"  TEXT,
"Phone"  TEXT,
"BelongOfficeName"  TEXT,
"CountryPosition"  TEXT,
"JiangSuPosition"  TEXT,
"IsPush"  INTEGER
);

-- ----------------------------
-- Table structure for CarBasicInfo
-- ----------------------------
DROP TABLE IF EXISTS "main"."CarBasicInfo";
CREATE TABLE "CarBasicInfo" (
"ID"  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
"RowGuid"  TEXT NOT NULL,
"CarNO"  TEXT
);

-- ----------------------------
-- Table structure for CityInfo
-- ----------------------------
DROP TABLE IF EXISTS "main"."CityInfo";
CREATE TABLE "CityInfo" (
"ID"  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
"RowGuid"  TEXT,
"CityName"  TEXT,
"AliasName"  TEXT,
"CityCode"  TEXT,
"OfficeName"  TEXT,
"BelongOfficeName"  TEXT,
"BelongCityCode"  TEXT,
"CountryPosition"  TEXT,
"JiangSuPosition"  TEXT
);

-- ----------------------------
-- Table structure for FrameConfig
-- ----------------------------
DROP TABLE IF EXISTS "main"."FrameConfig";
CREATE TABLE "FrameConfig" (
"ID"  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
"RowGuid"  TEXT NOT NULL,
"ConfigName"  TEXT,
"ConfigValue"  TEXT
);

-- ----------------------------
-- Table structure for FrameUser
-- ----------------------------
DROP TABLE IF EXISTS "main"."FrameUser";
CREATE TABLE "FrameUser" (
"ID"  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
"RowGuid"  TEXT NOT NULL,
"UserName"  TEXT NOT NULL,
"UserPassword"  TEXT NOT NULL,
"DisplayName"  TEXT,
"Mobile"  TEXT,
"Address"  TEXT,
"Birthday"  TEXT
);

-- ----------------------------
-- Table structure for InInfo
-- ----------------------------
DROP TABLE IF EXISTS "main"."InInfo";
CREATE TABLE "InInfo" (
"ID"  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
"RowGuid"  TEXT NOT NULL,
"AddDate"  TEXT,
"PostDate"  TEXT,
"MailNO"  TEXT,
"OrgName"  TEXT,
"Consignee"  TEXT,
"Address"  TEXT,
"Phone"  TEXT,
"IsPush"  INTEGER
);

-- ----------------------------
-- Table structure for InInfoHistory
-- ----------------------------
DROP TABLE IF EXISTS "main"."InInfoHistory";
CREATE TABLE "InInfoHistory" (
"ID"  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
"RowGuid"  TEXT NOT NULL,
"AddDate"  TEXT,
"PostDate"  TEXT,
"MailNO"  TEXT,
"OrgName"  TEXT,
"Consignee"  TEXT,
"Address"  TEXT,
"Phone"  TEXT,
"IsPush"  INTEGER
);

-- ----------------------------
-- Table structure for LabelBasicInfo
-- ----------------------------
DROP TABLE IF EXISTS "main"."LabelBasicInfo";
CREATE TABLE "LabelBasicInfo" (
"ID"  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
"RowGuid"  TEXT NOT NULL,
"LabelNO"  TEXT
);

-- ----------------------------
-- Table structure for LoginHistory
-- ----------------------------
DROP TABLE IF EXISTS "main"."LoginHistory";
CREATE TABLE "LoginHistory" (
"ID"  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
"RowGuid"  TEXT NOT NULL,
"LoginDisplayName"  TEXT,
"LoginUserName"  TEXT,
"LoginDate"  TEXT,
"IsPush"  INTEGER
);

-- ----------------------------
-- Table structure for OutInfo
-- ----------------------------
DROP TABLE IF EXISTS "main"."OutInfo";
CREATE TABLE "OutInfo" (
"ID"  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
"RowGuid"  TEXT NOT NULL,
"AddDate"  TEXT,
"PostDate"  TEXT,
"MailNO"  TEXT,
"OrgName"  TEXT,
"Consignee"  TEXT,
"Address"  TEXT,
"Phone"  TEXT,
"IsPush"  INTEGER,
"BelongOfficeName"  TEXT,
"CountryPosition"  TEXT,
"JiangSuPosition"  TEXT
);

-- ----------------------------
-- Table structure for OutInfoHistory
-- ----------------------------
DROP TABLE IF EXISTS "main"."OutInfoHistory";
CREATE TABLE "OutInfoHistory" (
"ID"  INTEGER PRIMARY KEY AUTOINCREMENT NOT NULL,
"RowGuid"  TEXT NOT NULL,
"AddDate"  TEXT,
"PostDate"  TEXT,
"MailNO"  TEXT,
"OrgName"  TEXT,
"Consignee"  TEXT,
"Address"  TEXT,
"Phone"  TEXT,
"IsPush"  INTEGER,
"BelongOfficeName"  TEXT,
"CountryPosition"  TEXT,
"JiangSuPosition"  TEXT
);

-- ----------------------------
-- Table structure for sqlite_sequence
-- ----------------------------
DROP TABLE IF EXISTS "main"."sqlite_sequence";
CREATE TABLE sqlite_sequence(name,seq);
