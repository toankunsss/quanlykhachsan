-- MySQL dump 10.13  Distrib 8.0.38, for Win64 (x86_64)
--
-- Host: localhost    Database: hotelmanage
-- ------------------------------------------------------
-- Server version	8.0.39

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `chitietphieuthue`
--

DROP TABLE IF EXISTS `chitietphieuthue`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `chitietphieuthue` (
  `MaPhieuThue` varchar(45) NOT NULL,
  `MaPhong` varchar(45) NOT NULL,
  `MaPhieuDichVu` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`MaPhieuThue`,`MaPhong`),
  KEY `FKDichVuCT_idx` (`MaPhieuDichVu`),
  KEY `FKMaPhong_idx` (`MaPhong`),
  CONSTRAINT `FKDichVuCT` FOREIGN KEY (`MaPhieuDichVu`) REFERENCES `phieudichvu` (`MaPhieuThue`),
  CONSTRAINT `FKMaPhong` FOREIGN KEY (`MaPhong`) REFERENCES `phong` (`MaPhong`),
  CONSTRAINT `FKPhieuThueCT` FOREIGN KEY (`MaPhieuThue`) REFERENCES `phieuthue` (`MaPhieuThue`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `chitietphieuthue`
--

LOCK TABLES `chitietphieuthue` WRITE;
/*!40000 ALTER TABLE `chitietphieuthue` DISABLE KEYS */;
/*!40000 ALTER TABLE `chitietphieuthue` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `dichvu`
--

DROP TABLE IF EXISTS `dichvu`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `dichvu` (
  `MaDV` varchar(45) NOT NULL,
  `TenDV` varchar(45) NOT NULL,
  `LoaiDV` varchar(45) NOT NULL,
  `DonGia` double NOT NULL,
  PRIMARY KEY (`MaDV`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `dichvu`
--

LOCK TABLES `dichvu` WRITE;
/*!40000 ALTER TABLE `dichvu` DISABLE KEYS */;
/*!40000 ALTER TABLE `dichvu` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `hoadon`
--

DROP TABLE IF EXISTS `hoadon`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `hoadon` (
  `MaPhieuThue` varchar(45) NOT NULL,
  `TongTien` double NOT NULL,
  `ngaythanhtoan` date NOT NULL,
  `MaNhanVien` varchar(45) NOT NULL,
  PRIMARY KEY (`MaPhieuThue`),
  KEY `FKMaNV_idx` (`MaNhanVien`),
  CONSTRAINT `FKMaNV` FOREIGN KEY (`MaNhanVien`) REFERENCES `nhanvien` (`MaNV`),
  CONSTRAINT `FKMaPhieuThueCT` FOREIGN KEY (`MaPhieuThue`) REFERENCES `phieuthue` (`MaPhieuThue`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `hoadon`
--

LOCK TABLES `hoadon` WRITE;
/*!40000 ALTER TABLE `hoadon` DISABLE KEYS */;
/*!40000 ALTER TABLE `hoadon` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `khachhang`
--

DROP TABLE IF EXISTS `khachhang`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `khachhang` (
  `KhCCCD` varchar(45) NOT NULL,
  `TenKH` varchar(255) NOT NULL,
  `ngaysinh` date NOT NULL,
  `DiaChi` varchar(45) NOT NULL,
  `SoDT` bigint NOT NULL,
  `GioiTinh` varchar(45) NOT NULL,
  PRIMARY KEY (`KhCCCD`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `khachhang`
--

LOCK TABLES `khachhang` WRITE;
/*!40000 ALTER TABLE `khachhang` DISABLE KEYS */;
/*!40000 ALTER TABLE `khachhang` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `loaiphong`
--

DROP TABLE IF EXISTS `loaiphong`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `loaiphong` (
  `Maloaiphong` varchar(45) NOT NULL,
  `TenLoai` varchar(45) NOT NULL,
  `songuoi` int NOT NULL,
  `dongia` double NOT NULL,
  PRIMARY KEY (`Maloaiphong`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `loaiphong`
--

LOCK TABLES `loaiphong` WRITE;
/*!40000 ALTER TABLE `loaiphong` DISABLE KEYS */;
/*!40000 ALTER TABLE `loaiphong` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `nhanvien`
--

DROP TABLE IF EXISTS `nhanvien`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `nhanvien` (
  `MaNV` varchar(45) NOT NULL,
  `TenNV` varchar(255) NOT NULL,
  `CCCD` bigint NOT NULL,
  `ngaysinh` date NOT NULL,
  `GioiTinh` varchar(45) NOT NULL,
  `Địa chỉ` varchar(45) NOT NULL,
  `SoDT` bigint NOT NULL,
  `ChucVu` varchar(45) NOT NULL,
  `MatKhau` varchar(45) NOT NULL,
  PRIMARY KEY (`MaNV`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `nhanvien`
--

LOCK TABLES `nhanvien` WRITE;
/*!40000 ALTER TABLE `nhanvien` DISABLE KEYS */;
/*!40000 ALTER TABLE `nhanvien` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `phieudichvu`
--

DROP TABLE IF EXISTS `phieudichvu`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `phieudichvu` (
  `MaPhieuThue` varchar(45) NOT NULL,
  `MaDV` varchar(45) NOT NULL,
  `SoLuong` int DEFAULT NULL,
  `thanhtien` double NOT NULL,
  PRIMARY KEY (`MaPhieuThue`,`MaDV`),
  KEY `FKDichVu_idx` (`MaDV`),
  CONSTRAINT `FKDichVu` FOREIGN KEY (`MaDV`) REFERENCES `dichvu` (`MaDV`),
  CONSTRAINT `FKPhieuThue` FOREIGN KEY (`MaPhieuThue`) REFERENCES `phieuthue` (`MaPhieuThue`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `phieudichvu`
--

LOCK TABLES `phieudichvu` WRITE;
/*!40000 ALTER TABLE `phieudichvu` DISABLE KEYS */;
/*!40000 ALTER TABLE `phieudichvu` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `phieuthue`
--

DROP TABLE IF EXISTS `phieuthue`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `phieuthue` (
  `MaPhieuThue` varchar(45) NOT NULL,
  `MaKH` varchar(45) NOT NULL,
  `ngayden` date NOT NULL,
  `ngaydi` date NOT NULL,
  PRIMARY KEY (`MaPhieuThue`),
  KEY `FKMaKH_idx` (`MaKH`),
  CONSTRAINT `FKKhachHang` FOREIGN KEY (`MaKH`) REFERENCES `khachhang` (`KhCCCD`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `phieuthue`
--

LOCK TABLES `phieuthue` WRITE;
/*!40000 ALTER TABLE `phieuthue` DISABLE KEYS */;
/*!40000 ALTER TABLE `phieuthue` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `phong`
--

DROP TABLE IF EXISTS `phong`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `phong` (
  `MaPhong` varchar(45) NOT NULL,
  `MaLoaiPhong` varchar(45) NOT NULL,
  `TinhTrang` varchar(45) NOT NULL,
  `GhiChu` varchar(45) DEFAULT NULL,
  PRIMARY KEY (`MaPhong`),
  KEY `FKLoaiPhong_idx` (`MaLoaiPhong`),
  CONSTRAINT `FKLoaiPhong` FOREIGN KEY (`MaLoaiPhong`) REFERENCES `loaiphong` (`Maloaiphong`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `phong`
--

LOCK TABLES `phong` WRITE;
/*!40000 ALTER TABLE `phong` DISABLE KEYS */;
/*!40000 ALTER TABLE `phong` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2024-11-16 14:26:43
