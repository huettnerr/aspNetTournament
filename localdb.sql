-- phpMyAdmin SQL Dump
-- version 4.6.6
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1:52827
-- Generation Time: Sep 16, 2023 at 11:40 AM
-- Server version: 5.7.9-log
-- PHP Version: 5.6.40

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `localdb`
--

-- --------------------------------------------------------

--
-- Table structure for table `groups`
--

CREATE TABLE `groups` (
  `groupId` int(11) NOT NULL,
  `roundId` int(11) NOT NULL,
  `name` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `groups`
--

INSERT INTO `groups` (`groupId`, `roundId`, `name`) VALUES
(47, 5, 'A'),
(48, 5, 'B'),
(49, 5, 'C'),
(50, 5, 'D'),
(51, 5, 'E'),
(52, 5, 'F'),
(53, 5, 'G'),
(54, 5, 'H'),
(55, 6, 'Sternburg'),
(56, 6, 'Coschützer'),
(57, 6, 'Tyskie'),
(58, 6, 'Pilsner Urquell'),
(59, 6, 'Oettinger Pils'),
(60, 6, 'Staropramen'),
(61, 6, 'Ur-Krostitzer'),
(62, 6, 'Feldschlösschen'),
(70, 7, 'Stufe der besten 16'),
(71, 7, 'Stufe der besten 8'),
(72, 7, 'Stufe der besten 4'),
(73, 7, 'Stufe der besten 2'),
(74, 8, 'Alice Cooper'),
(75, 8, 'Black Sabbath'),
(76, 8, 'Cinderella'),
(77, 8, 'Dokken'),
(78, 8, 'Europe'),
(79, 8, 'Foreigner'),
(80, 8, 'Iron Maiden'),
(81, 8, 'Judas Priest'),
(105, 11, 'Stufe der besten 16'),
(106, 11, 'Viertelfinale'),
(107, 11, 'Halbfinale'),
(108, 11, 'Finale'),
(111, 13, 'Stufe der besten 16'),
(112, 13, 'Viertelfinale'),
(113, 13, 'Halbfinale'),
(114, 13, 'Finale');

-- --------------------------------------------------------

--
-- Table structure for table `map_round_venue`
--

CREATE TABLE `map_round_venue` (
  `rvMapId` int(11) NOT NULL,
  `roundId` int(11) NOT NULL,
  `venueId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `map_round_venue`
--

INSERT INTO `map_round_venue` (`rvMapId`, `roundId`, `venueId`) VALUES
(1, 6, 5),
(2, 6, 6),
(3, 6, 7),
(4, 6, 8),
(5, 6, 9),
(6, 5, 5),
(7, 5, 6),
(8, 7, 5),
(9, 7, 6),
(10, 7, 7),
(11, 7, 8),
(12, 7, 9),
(13, 5, 7),
(14, 8, 5),
(15, 8, 6),
(16, 8, 7),
(17, 8, 8),
(18, 8, 9),
(19, 11, 5),
(20, 11, 6),
(21, 11, 7),
(22, 11, 8),
(23, 11, 9);

-- --------------------------------------------------------

--
-- Table structure for table `map_tournament_seed_player`
--

CREATE TABLE `map_tournament_seed_player` (
  `tpMapId` int(11) NOT NULL,
  `tournamentId` int(11) NOT NULL,
  `seedId` int(11) NOT NULL,
  `playerId` int(11) DEFAULT NULL,
  `playerFixed` tinyint(1) NOT NULL DEFAULT '0',
  `playerCheckedIn` tinyint(1) NOT NULL DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `map_tournament_seed_player`
--

INSERT INTO `map_tournament_seed_player` (`tpMapId`, `tournamentId`, `seedId`, `playerId`, `playerFixed`, `playerCheckedIn`) VALUES
(41, 2, 160, 46, 0, 0),
(42, 2, 161, 32, 0, 0),
(43, 2, 162, 47, 0, 0),
(44, 2, 163, 28, 0, 0),
(45, 2, 164, 49, 0, 0),
(46, 2, 165, 20, 0, 0),
(47, 2, 166, 21, 0, 0),
(48, 2, 167, 23, 0, 0),
(49, 2, 168, 30, 0, 0),
(50, 2, 169, 29, 0, 0),
(51, 2, 170, 27, 0, 0),
(52, 2, 171, 35, 0, 0),
(53, 2, 172, 22, 0, 0),
(54, 2, 173, 36, 0, 0),
(55, 2, 174, 13, 0, 0),
(56, 2, 175, 17, 0, 0),
(57, 2, 176, 51, 0, 0),
(58, 2, 177, 41, 0, 0),
(59, 2, 178, 48, 0, 0),
(60, 2, 179, 50, 0, 0),
(61, 2, 180, 12, 0, 0),
(62, 2, 181, 24, 0, 0),
(63, 2, 182, 42, 0, 0),
(64, 2, 183, 37, 0, 0),
(65, 2, 184, 11, 0, 0),
(66, 2, 185, 33, 0, 0),
(67, 2, 186, 26, 0, 0),
(68, 2, 187, 44, 0, 0),
(69, 3, 188, 24, 0, 0),
(70, 3, 189, 37, 0, 0),
(71, 3, 190, 36, 0, 0),
(72, 3, 191, 14, 0, 0),
(73, 3, 192, 26, 0, 0),
(74, 3, 193, 11, 0, 0),
(75, 3, 194, 38, 0, 0),
(76, 3, 195, 41, 0, 0),
(77, 3, 196, 17, 0, 0),
(78, 3, 197, 43, 0, 0),
(79, 3, 198, 42, 0, 0),
(80, 3, 199, 44, 0, 0),
(81, 3, 200, 22, 0, 0),
(82, 3, 201, 45, 0, 0),
(83, 3, 202, 15, 0, 0),
(84, 3, 203, 16, 0, 0),
(85, 3, 204, 27, 0, 0),
(86, 3, 205, 8, 0, 1),
(87, 3, 206, 7, 0, 0),
(88, 3, 207, 10, 0, 0),
(89, 3, 208, 23, 0, 0),
(90, 3, 209, 20, 0, 0),
(91, 3, 210, 31, 0, 0),
(92, 3, 211, 29, 0, 0),
(93, 3, 212, 25, 0, 0),
(94, 3, 213, 39, 0, 0),
(95, 3, 214, 18, 0, 0),
(96, 3, 215, 13, 0, 0),
(97, 3, 216, 21, 0, 0),
(98, 3, 217, 33, 0, 0),
(99, 3, 218, 28, 0, 0),
(100, 3, 219, 40, 0, 0),
(101, 4, 294, 33, 0, 0),
(102, 4, 295, 31, 0, 0),
(103, 4, 296, 7, 0, 0),
(104, 4, 297, 11, 0, 0),
(105, 4, 298, 51, 0, 0),
(106, 4, 299, 42, 0, 0),
(107, 4, 300, 44, 0, 0),
(108, 4, 301, 37, 0, 0),
(109, 4, 302, 53, 0, 0),
(110, 4, 303, 27, 0, 0),
(111, 4, 304, 49, 0, 0),
(112, 4, 305, 14, 0, 0),
(113, 4, 306, 8, 0, 0),
(114, 4, 307, 20, 0, 0),
(115, 4, 308, 52, 0, 0),
(116, 4, 309, 47, 0, 0),
(117, 4, 310, 41, 0, 0),
(118, 4, 311, 48, 0, 0),
(119, 4, 312, 24, 0, 0),
(121, 4, 314, 10, 0, 0),
(122, 4, 315, 26, 0, 0),
(123, 4, 316, 13, 0, 0),
(125, 4, 318, 9, 0, 0),
(126, 4, 319, 29, 0, 0),
(127, 4, 320, 23, 0, 0),
(129, 4, 322, 50, 0, 0),
(130, 4, 323, 22, 0, 0),
(131, 4, 324, 46, 0, 0);

-- --------------------------------------------------------

--
-- Table structure for table `matches`
--

CREATE TABLE `matches` (
  `matchId` int(11) NOT NULL,
  `matchOrderValue` int(11) DEFAULT NULL,
  `groupId` int(11) NOT NULL,
  `seed1Id` int(11) NOT NULL,
  `seed2Id` int(11) NOT NULL,
  `status` enum('Created','Active','Finished','Aborted') NOT NULL DEFAULT 'Created',
  `venueId` int(11) DEFAULT NULL,
  `time_started` datetime DEFAULT NULL,
  `time_finished` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `matches`
--

INSERT INTO `matches` (`matchId`, `matchOrderValue`, `groupId`, `seed1Id`, `seed2Id`, `status`, `venueId`, `time_started`, `time_finished`) VALUES
(112, 1, 55, 188, 189, 'Finished', NULL, NULL, NULL),
(113, 1, 55, 190, 191, 'Finished', NULL, NULL, NULL),
(114, 3, 55, 188, 190, 'Finished', NULL, NULL, NULL),
(115, 3, 55, 189, 191, 'Finished', NULL, NULL, NULL),
(116, 5, 55, 188, 191, 'Finished', NULL, NULL, NULL),
(117, 5, 55, 189, 190, 'Finished', NULL, NULL, NULL),
(118, 1, 56, 192, 193, 'Finished', NULL, NULL, NULL),
(119, 1, 56, 194, 195, 'Finished', NULL, NULL, NULL),
(120, 3, 56, 192, 194, 'Finished', NULL, NULL, NULL),
(121, 3, 56, 193, 195, 'Finished', NULL, NULL, NULL),
(122, 5, 56, 192, 195, 'Finished', NULL, NULL, NULL),
(123, 5, 56, 193, 194, 'Finished', NULL, NULL, NULL),
(124, 1, 57, 196, 197, 'Finished', NULL, NULL, NULL),
(125, 1, 57, 198, 199, 'Finished', NULL, NULL, NULL),
(126, 3, 57, 196, 198, 'Finished', NULL, NULL, NULL),
(127, 3, 57, 197, 199, 'Finished', NULL, NULL, NULL),
(128, 5, 57, 196, 199, 'Finished', NULL, NULL, NULL),
(129, 5, 57, 197, 198, 'Finished', NULL, NULL, NULL),
(130, 1, 58, 200, 201, 'Finished', NULL, NULL, NULL),
(131, 1, 58, 202, 203, 'Finished', NULL, NULL, NULL),
(132, 3, 58, 200, 202, 'Finished', NULL, NULL, NULL),
(133, 3, 58, 201, 203, 'Finished', NULL, NULL, NULL),
(134, 5, 58, 200, 203, 'Finished', NULL, NULL, NULL),
(135, 5, 58, 201, 202, 'Finished', NULL, NULL, NULL),
(136, 1, 59, 204, 205, 'Finished', NULL, NULL, NULL),
(137, 1, 59, 206, 207, 'Finished', NULL, NULL, NULL),
(138, 3, 59, 204, 206, 'Finished', NULL, NULL, NULL),
(139, 3, 59, 205, 207, 'Finished', NULL, NULL, NULL),
(140, 5, 59, 204, 207, 'Finished', NULL, NULL, NULL),
(141, 5, 59, 205, 206, 'Finished', NULL, NULL, NULL),
(142, 1, 60, 208, 209, 'Finished', NULL, NULL, NULL),
(143, 1, 60, 210, 211, 'Finished', NULL, NULL, NULL),
(144, 3, 60, 208, 210, 'Finished', NULL, NULL, NULL),
(145, 3, 60, 209, 211, 'Finished', NULL, NULL, NULL),
(146, 5, 60, 208, 211, 'Finished', NULL, NULL, NULL),
(147, 5, 60, 209, 210, 'Finished', NULL, NULL, NULL),
(148, 1, 61, 212, 213, 'Finished', NULL, NULL, NULL),
(149, 1, 61, 214, 215, 'Finished', NULL, NULL, NULL),
(150, 3, 61, 212, 214, 'Finished', NULL, NULL, NULL),
(151, 3, 61, 213, 215, 'Finished', NULL, NULL, NULL),
(152, 5, 61, 212, 215, 'Finished', NULL, NULL, NULL),
(153, 5, 61, 213, 214, 'Finished', NULL, NULL, NULL),
(154, 1, 62, 216, 217, 'Finished', NULL, NULL, NULL),
(155, 1, 62, 218, 219, 'Finished', NULL, NULL, NULL),
(156, 3, 62, 216, 218, 'Finished', NULL, NULL, NULL),
(157, 3, 62, 217, 219, 'Finished', NULL, NULL, NULL),
(158, 5, 62, 216, 219, 'Finished', NULL, NULL, NULL),
(159, 5, 62, 217, 218, 'Finished', NULL, NULL, NULL),
(167, 0, 70, 189, 216, 'Finished', NULL, NULL, NULL),
(168, 1, 70, 195, 213, 'Finished', NULL, NULL, NULL),
(169, 2, 70, 199, 208, 'Finished', NULL, NULL, NULL),
(170, 3, 70, 202, 205, 'Finished', NULL, NULL, NULL),
(171, 4, 70, 204, 203, 'Finished', NULL, NULL, NULL),
(172, 5, 70, 210, 197, 'Finished', NULL, NULL, NULL),
(173, 6, 70, 215, 194, 'Finished', NULL, NULL, NULL),
(174, 7, 70, 218, 191, 'Finished', NULL, NULL, NULL),
(175, 0, 71, 189, 195, 'Finished', NULL, NULL, NULL),
(176, 1, 71, 199, 202, 'Finished', NULL, NULL, NULL),
(177, 2, 71, 204, 210, 'Finished', NULL, NULL, NULL),
(178, 3, 71, 215, 218, 'Finished', NULL, NULL, NULL),
(179, 0, 72, 195, 199, 'Finished', NULL, NULL, NULL),
(180, 1, 72, 210, 215, 'Finished', NULL, NULL, NULL),
(181, 0, 73, 195, 210, 'Finished', NULL, NULL, NULL),
(366, 1, 74, 294, 295, 'Finished', NULL, NULL, NULL),
(367, 1, 74, 296, 297, 'Finished', NULL, NULL, NULL),
(368, 3, 74, 294, 296, 'Finished', NULL, NULL, NULL),
(369, 3, 74, 295, 297, 'Finished', NULL, NULL, NULL),
(370, 5, 74, 294, 297, 'Finished', NULL, NULL, NULL),
(371, 5, 74, 295, 296, 'Finished', NULL, NULL, NULL),
(372, 1, 75, 298, 299, 'Finished', NULL, NULL, NULL),
(373, 1, 75, 300, 301, 'Finished', NULL, NULL, NULL),
(374, 3, 75, 298, 300, 'Finished', NULL, NULL, NULL),
(375, 3, 75, 299, 301, 'Finished', NULL, NULL, NULL),
(376, 5, 75, 298, 301, 'Finished', NULL, NULL, NULL),
(377, 5, 75, 299, 300, 'Finished', NULL, NULL, NULL),
(378, 1, 76, 302, 303, 'Finished', NULL, NULL, NULL),
(379, 1, 76, 304, 305, 'Finished', NULL, NULL, NULL),
(380, 3, 76, 302, 304, 'Finished', NULL, NULL, NULL),
(381, 3, 76, 303, 305, 'Finished', NULL, NULL, NULL),
(382, 5, 76, 302, 305, 'Finished', NULL, NULL, NULL),
(383, 5, 76, 303, 304, 'Finished', NULL, NULL, NULL),
(384, 1, 77, 306, 307, 'Finished', NULL, NULL, NULL),
(385, 1, 77, 308, 309, 'Finished', NULL, NULL, NULL),
(386, 3, 77, 306, 308, 'Finished', NULL, NULL, NULL),
(387, 3, 77, 307, 309, 'Finished', NULL, NULL, NULL),
(388, 5, 77, 306, 309, 'Finished', NULL, NULL, NULL),
(389, 5, 77, 307, 308, 'Finished', NULL, NULL, NULL),
(390, 0, 78, 310, 311, 'Finished', NULL, NULL, NULL),
(391, 1, 78, 311, 312, 'Finished', NULL, NULL, NULL),
(392, 2, 78, 310, 312, 'Finished', NULL, NULL, NULL),
(393, 3, 78, 312, 311, 'Finished', NULL, NULL, NULL),
(394, 4, 78, 312, 310, 'Finished', NULL, NULL, NULL),
(395, 5, 78, 311, 310, 'Finished', NULL, NULL, NULL),
(396, 0, 79, 314, 315, 'Finished', NULL, NULL, NULL),
(397, 1, 79, 315, 316, 'Finished', NULL, NULL, NULL),
(398, 2, 79, 314, 316, 'Finished', NULL, NULL, NULL),
(399, 3, 79, 316, 315, 'Finished', NULL, NULL, NULL),
(400, 4, 79, 316, 314, 'Finished', NULL, NULL, NULL),
(401, 5, 79, 315, 314, 'Finished', NULL, NULL, NULL),
(402, 0, 80, 318, 319, 'Finished', NULL, NULL, NULL),
(403, 1, 80, 319, 320, 'Finished', NULL, NULL, NULL),
(404, 2, 80, 318, 320, 'Finished', NULL, NULL, NULL),
(405, 3, 80, 320, 319, 'Finished', NULL, NULL, NULL),
(406, 4, 80, 320, 318, 'Finished', NULL, NULL, NULL),
(407, 5, 80, 319, 318, 'Finished', NULL, NULL, NULL),
(408, 0, 81, 322, 323, 'Finished', NULL, NULL, NULL),
(409, 1, 81, 323, 324, 'Finished', NULL, NULL, NULL),
(410, 2, 81, 322, 324, 'Finished', NULL, NULL, NULL),
(411, 3, 81, 324, 323, 'Finished', NULL, NULL, NULL),
(412, 4, 81, 324, 322, 'Finished', NULL, NULL, NULL),
(413, 5, 81, 323, 322, 'Finished', NULL, NULL, NULL),
(414, 0, 105, 296, 324, 'Finished', NULL, NULL, NULL),
(415, 1, 105, 301, 319, 'Finished', NULL, NULL, NULL),
(416, 2, 105, 305, 315, 'Finished', NULL, NULL, NULL),
(417, 3, 105, 309, 312, 'Finished', NULL, NULL, NULL),
(418, 4, 105, 310, 307, 'Finished', NULL, NULL, NULL),
(419, 5, 105, 314, 304, 'Finished', NULL, NULL, NULL),
(420, 6, 105, 320, 298, 'Finished', NULL, NULL, NULL),
(421, 7, 105, 322, 295, 'Finished', NULL, NULL, NULL),
(422, 0, 106, 296, 301, 'Finished', NULL, NULL, NULL),
(423, 1, 106, 305, 309, 'Finished', NULL, NULL, NULL),
(424, 2, 106, 310, 314, 'Finished', NULL, NULL, NULL),
(425, 3, 106, 320, 322, 'Finished', NULL, NULL, NULL),
(426, 0, 107, 301, 309, 'Finished', NULL, NULL, NULL),
(427, 1, 107, 310, 322, 'Finished', NULL, NULL, NULL),
(428, 0, 108, 309, 310, 'Finished', NULL, NULL, NULL),
(432, 1, 47, 160, 161, 'Created', NULL, NULL, NULL),
(433, 1, 47, 162, 163, 'Created', NULL, NULL, NULL),
(434, 3, 47, 160, 162, 'Created', NULL, NULL, NULL),
(435, 3, 47, 161, 163, 'Created', NULL, NULL, NULL),
(436, 5, 47, 160, 163, 'Created', NULL, NULL, NULL),
(437, 5, 47, 161, 162, 'Created', NULL, NULL, NULL),
(438, 1, 48, 164, 165, 'Created', NULL, NULL, NULL),
(439, 1, 48, 166, 167, 'Created', NULL, NULL, NULL),
(440, 3, 48, 164, 166, 'Created', NULL, NULL, NULL),
(441, 3, 48, 165, 167, 'Created', NULL, NULL, NULL),
(442, 5, 48, 164, 167, 'Created', NULL, NULL, NULL),
(443, 5, 48, 165, 166, 'Created', NULL, NULL, NULL),
(444, 1, 49, 168, 169, 'Created', NULL, NULL, NULL),
(445, 1, 49, 170, 171, 'Created', NULL, NULL, NULL),
(446, 3, 49, 168, 170, 'Created', NULL, NULL, NULL),
(447, 3, 49, 169, 171, 'Created', NULL, NULL, NULL),
(448, 5, 49, 168, 171, 'Created', NULL, NULL, NULL),
(449, 5, 49, 169, 170, 'Created', NULL, NULL, NULL),
(450, 1, 50, 172, 173, 'Created', NULL, NULL, NULL),
(451, 1, 50, 174, 175, 'Created', NULL, NULL, NULL),
(452, 3, 50, 172, 174, 'Created', NULL, NULL, NULL),
(453, 3, 50, 173, 175, 'Created', NULL, NULL, NULL),
(454, 5, 50, 172, 175, 'Created', NULL, NULL, NULL),
(455, 5, 50, 173, 174, 'Created', NULL, NULL, NULL),
(456, 0, 51, 176, 177, 'Active', NULL, NULL, NULL),
(457, 1, 51, 177, 178, 'Created', NULL, NULL, NULL),
(458, 2, 51, 176, 178, 'Created', NULL, NULL, NULL),
(459, 3, 51, 178, 177, 'Created', NULL, NULL, NULL),
(460, 4, 51, 178, 176, 'Created', NULL, NULL, NULL),
(461, 5, 51, 177, 176, 'Created', NULL, NULL, NULL),
(462, 0, 52, 179, 180, 'Created', NULL, NULL, NULL),
(463, 1, 52, 180, 181, 'Created', NULL, NULL, NULL),
(464, 2, 52, 179, 181, 'Created', NULL, NULL, NULL),
(465, 3, 52, 181, 180, 'Created', NULL, NULL, NULL),
(466, 4, 52, 181, 179, 'Created', NULL, NULL, NULL),
(467, 5, 52, 180, 179, 'Created', NULL, NULL, NULL),
(468, 0, 53, 182, 183, 'Created', NULL, NULL, NULL),
(469, 1, 53, 183, 184, 'Created', NULL, NULL, NULL),
(470, 2, 53, 182, 184, 'Created', NULL, NULL, NULL),
(471, 3, 53, 184, 183, 'Created', NULL, NULL, NULL),
(472, 4, 53, 184, 182, 'Created', NULL, NULL, NULL),
(473, 5, 53, 183, 182, 'Created', NULL, NULL, NULL),
(474, 0, 54, 185, 186, 'Created', NULL, NULL, NULL),
(475, 1, 54, 186, 187, 'Created', NULL, NULL, NULL),
(476, 2, 54, 185, 187, 'Created', NULL, NULL, NULL),
(477, 3, 54, 187, 186, 'Created', NULL, NULL, NULL),
(478, 4, 54, 187, 185, 'Created', NULL, NULL, NULL),
(479, 5, 54, 186, 185, 'Created', NULL, NULL, NULL),
(480, 0, 111, 160, 186, 'Active', NULL, NULL, NULL),
(481, 1, 111, 164, 183, 'Created', NULL, NULL, NULL),
(482, 2, 111, 168, 180, 'Created', NULL, NULL, NULL),
(483, 3, 111, 172, 177, 'Created', NULL, NULL, NULL),
(484, 4, 111, 176, 173, 'Created', NULL, NULL, NULL),
(485, 5, 111, 179, 169, 'Created', NULL, NULL, NULL),
(486, 6, 111, 182, 165, 'Created', NULL, NULL, NULL),
(487, 7, 111, 185, 161, 'Created', NULL, NULL, NULL),
(488, 0, 112, 542, 543, 'Created', NULL, NULL, NULL),
(489, 1, 112, 544, 545, 'Created', NULL, NULL, NULL),
(490, 2, 112, 546, 547, 'Created', NULL, NULL, NULL),
(491, 3, 112, 548, 549, 'Created', NULL, NULL, NULL),
(492, 0, 113, 550, 551, 'Created', NULL, NULL, NULL),
(493, 1, 113, 552, 553, 'Created', NULL, NULL, NULL),
(494, 0, 114, 554, 555, 'Created', NULL, NULL, NULL);

-- --------------------------------------------------------

--
-- Table structure for table `players`
--

CREATE TABLE `players` (
  `playerId` int(11) NOT NULL,
  `name` text NOT NULL,
  `dartname` text,
  `contactData` text,
  `interpret` text,
  `song` text
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `players`
--

INSERT INTO `players` (`playerId`, `name`, `dartname`, `contactData`, `interpret`, `song`) VALUES
(7, 'Ronny Rotator', 'Turning Tables', NULL, 'Snake', 'Day of Solution'),
(8, 'Ralle', 'Regenbogenfo', NULL, 'Scooter', 'How much is the fish?'),
(9, 'Steve\'o', 'Score Or Die', NULL, NULL, NULL),
(10, 'Sebastian', 'Basstart', NULL, 'Driller Killer', 'Skaneland'),
(11, 'Mike', 'Der Leopard', NULL, 'Survivor', 'Eye of the Tiger'),
(12, 'Marie', 'MC Hammerschmidt', NULL, 'MC Hammer', 'Can\'t Touch This'),
(13, 'Glenn', 'Der fliegende Holländer', NULL, 'Wolfgang Petri', 'Der Himmel brennt'),
(14, 'Pascal', 'Ralle Export', NULL, 'Scooter', 'I like it loud'),
(15, 'Tom-Louis', 'Hawkcore', NULL, 'O*****', 'Stunde des Siegers'),
(16, 'Niebe', 'Der Unendliche', NULL, 'Klaus Doldinger', 'Flug auf dem Glücksdrachen'),
(17, 'Henning', 'Snooper', NULL, 'Iggy Pop', 'The Passenger'),
(18, 'Sascha', 'Ranger', NULL, 'Gigi D\'agostini', 'l\'amour tojours'),
(19, 'Tim', 'The Four-Eye', NULL, 'Biggi', 'Juicy'),
(20, 'Chrischan', 'Toddler', NULL, 'Fancy', 'Slice me nice'),
(21, 'David', 'The Caller', NULL, 'The Pharside', 'Passing me by'),
(22, 'Fred', 'Eisentod', NULL, 'Die Kassierer', 'Blumenkohl am Pillermann'),
(23, 'David', 'Coach-Atze 69', NULL, 'Mrk Foggo\'s Skasters', 'Car on a train'),
(24, 'Markus', 'Zitterhand', NULL, NULL, NULL),
(25, 'Stefan', 'Vögeldi', NULL, 'Joe Satriani', 'Shockwave Supernova'),
(26, 'Moritz', 'Krümelmännchen', NULL, 'Pixies', 'Rock Music'),
(27, 'Felix', 'Sherrif', NULL, 'The good, the Bad and the Ugly (Movie)', 'The Theme Song'),
(28, 'Basti', 'HansKlausFranz', NULL, 'Wham!', 'Last Christmas'),
(29, 'Eric', 'Roter Samu', NULL, 'Wolfgang Petri', 'Weiß der Geier'),
(30, 'Dennybube', NULL, NULL, NULL, NULL),
(31, 'Rico', 'Ziese', NULL, 'Asphalt Indianer', 'Zug'),
(32, 'Alexander H.', NULL, NULL, NULL, NULL),
(33, 'Mirko', 'Mirconan der Barbar', NULL, 'Isolierband', 'Keine Gnade'),
(34, 'Ralle', 'Nonkonform', NULL, NULL, NULL),
(35, 'Franz Z.', NULL, NULL, NULL, NULL),
(36, 'Fritz', 'Fritto', NULL, 'Canned Heat', 'Going up to the Country'),
(37, 'Maximilian ', 'The Fall Down Boy', NULL, 'Heaven Shall Burn', 'Black Tears'),
(38, 'Thomas', 'Captain Europe', NULL, NULL, 'Bibi und Tina (Madrid Remix)'),
(39, 'Thomas', 'Total Eclipse of the Dart', NULL, 'Horse Giirl', 'My white little Pony'),
(40, 'Robby', 'Bronko', NULL, 'Eminem', 'Slim Shady'),
(41, 'Leopold', 'Piri Piri', NULL, 'MARINA', 'Primadonna'),
(42, 'Martin', 'Erlero', NULL, 'Backstreet Boys', 'Everybody'),
(43, 'Philipp', 'The Bullett', NULL, '1. FC Nürnberg', 'Die Legende lebt'),
(44, 'Moritz', 'Wine Mania', NULL, 'Electric Callboy', 'Hyper Hyper'),
(45, 'Vadim', 'Strahlemann', NULL, '\"Serienintro\"', 'Sailermoon'),
(46, 'Alex Heinze', 'Ronny McHammergeil', NULL, 'Elsterglanz', 'Kaputtschaahn'),
(47, 'Andreas Gregor', 'The Tower', NULL, 'White Reaper', 'Raw'),
(48, 'Manu', 'The Flying Koggsman', NULL, 'Bon Jovi', 'Raise your Hands'),
(49, 'Chris', 'M3tl3r', NULL, 'Dicks on Fire', 'Superbad Motherfucker'),
(50, 'Marcel Davideit', 'The Lightning', NULL, 'MOP', 'Ante Up'),
(51, 'Jakob Piribauer', 'Siracha', NULL, NULL, NULL),
(52, 'Toni', 'Fit Tony', NULL, 'Broilers', 'Küss meinen Ring'),
(53, 'Pete', 'Grind Peter', NULL, 'Municipal Waste', 'Born to Party');

-- --------------------------------------------------------

--
-- Table structure for table `rounds`
--

CREATE TABLE `rounds` (
  `roundId` int(11) NOT NULL,
  `tournamentId` int(11) NOT NULL,
  `name` text NOT NULL,
  `modus` enum('RoundRobin','SingleKo','DoubleKo','') NOT NULL,
  `scoring` enum('SetsOnly','LegsOnly','SetsAndLegs') NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `rounds`
--

INSERT INTO `rounds` (`roundId`, `tournamentId`, `name`, `modus`, `scoring`) VALUES
(5, 2, 'Vorrunde', 'RoundRobin', 'LegsOnly'),
(6, 3, 'Vorgeplänkel', 'RoundRobin', 'LegsOnly'),
(7, 3, 'On Stage!', 'SingleKo', 'LegsOnly'),
(8, 4, 'Vorbands', 'RoundRobin', 'LegsOnly'),
(11, 4, 'On Stage!', 'SingleKo', 'LegsOnly'),
(12, 2, 'HR', 'RoundRobin', 'LegsOnly'),
(13, 2, 'HR2', 'SingleKo', 'LegsOnly');

-- --------------------------------------------------------

--
-- Table structure for table `scores`
--

CREATE TABLE `scores` (
  `scoreId` int(11) NOT NULL,
  `matchId` int(11) NOT NULL,
  `p1Sets` int(11) NOT NULL,
  `p2Sets` int(11) NOT NULL,
  `p1Legs` int(11) NOT NULL,
  `p2Legs` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `scores`
--

INSERT INTO `scores` (`scoreId`, `matchId`, `p1Sets`, `p2Sets`, `p1Legs`, `p2Legs`) VALUES
(4, 112, 0, 0, 0, 2),
(5, 113, 0, 0, 0, 2),
(6, 118, 0, 0, 2, 1),
(7, 119, 0, 0, 1, 2),
(8, 124, 0, 0, 1, 2),
(9, 125, 0, 0, 0, 2),
(10, 130, 0, 0, 0, 2),
(11, 131, 0, 0, 2, 0),
(12, 136, 0, 0, 1, 2),
(13, 137, 0, 0, 1, 2),
(14, 142, 0, 0, 2, 0),
(15, 143, 0, 0, 2, 0),
(16, 148, 0, 0, 0, 2),
(17, 149, 0, 0, 0, 2),
(18, 154, 0, 0, 2, 0),
(19, 155, 0, 0, 2, 0),
(20, 114, 0, 0, 1, 2),
(21, 115, 0, 0, 2, 0),
(22, 120, 0, 0, 1, 2),
(23, 121, 0, 0, 0, 2),
(24, 126, 0, 0, 0, 2),
(25, 127, 0, 0, 0, 2),
(26, 132, 0, 0, 0, 2),
(27, 133, 0, 0, 1, 2),
(28, 138, 0, 0, 2, 0),
(29, 139, 0, 0, 2, 0),
(30, 144, 0, 0, 1, 2),
(31, 145, 0, 0, 1, 2),
(32, 150, 0, 0, 0, 2),
(33, 151, 0, 0, 0, 2),
(34, 156, 0, 0, 0, 2),
(35, 157, 0, 0, 2, 1),
(36, 116, 0, 0, 1, 2),
(37, 117, 0, 0, 2, 0),
(38, 122, 0, 0, 2, 1),
(39, 123, 0, 0, 0, 2),
(40, 128, 0, 0, 0, 2),
(41, 129, 0, 0, 2, 0),
(42, 134, 0, 0, 0, 2),
(43, 135, 0, 0, 1, 2),
(44, 140, 0, 0, 2, 1),
(45, 141, 0, 0, 0, 2),
(46, 146, 0, 0, 2, 1),
(47, 147, 0, 0, 0, 2),
(48, 152, 0, 0, 0, 2),
(49, 153, 0, 0, 2, 0),
(50, 158, 0, 0, 2, 0),
(51, 159, 0, 0, 0, 2),
(52, 167, 0, 0, 2, 0),
(53, 168, 0, 0, 2, 0),
(54, 169, 0, 0, 2, 1),
(55, 170, 0, 0, 2, 1),
(56, 171, 0, 0, 2, 1),
(57, 172, 0, 0, 2, 0),
(58, 173, 0, 0, 2, 0),
(59, 174, 0, 0, 2, 1),
(60, 175, 0, 0, 2, 3),
(61, 176, 0, 0, 3, 0),
(62, 177, 0, 0, 0, 3),
(63, 178, 0, 0, 3, 2),
(64, 179, 0, 0, 3, 0),
(65, 180, 0, 0, 3, 2),
(66, 181, 0, 0, 4, 1),
(72, 390, 0, 0, 2, 1),
(73, 396, 0, 0, 2, 0),
(74, 402, 0, 0, 1, 2),
(75, 408, 0, 0, 2, 0),
(76, 366, 0, 0, 1, 2),
(77, 367, 0, 0, 2, 0),
(78, 372, 0, 0, 2, 0),
(79, 373, 0, 0, 1, 2),
(80, 378, 0, 0, 0, 2),
(81, 379, 0, 0, 2, 0),
(82, 384, 0, 0, 0, 2),
(83, 385, 0, 0, 1, 2),
(84, 391, 0, 0, 1, 2),
(85, 397, 0, 0, 2, 1),
(86, 403, 0, 0, 1, 2),
(87, 409, 0, 0, 0, 2),
(89, 398, 0, 0, 2, 0),
(90, 404, 0, 0, 0, 2),
(91, 392, 0, 0, 2, 0),
(92, 368, 0, 0, 1, 2),
(93, 369, 0, 0, 2, 0),
(94, 410, 0, 0, 2, 0),
(95, 374, 0, 0, 2, 0),
(96, 375, 0, 0, 0, 2),
(97, 380, 0, 0, 2, 0),
(98, 381, 0, 0, 0, 2),
(99, 386, 0, 0, 2, 0),
(100, 387, 0, 0, 1, 2),
(101, 393, 0, 0, 2, 0),
(102, 399, 0, 0, 2, 0),
(103, 405, 0, 0, 2, 1),
(104, 411, 0, 0, 2, 0),
(105, 370, 0, 0, 1, 2),
(106, 394, 0, 0, 0, 2),
(107, 400, 0, 0, 1, 2),
(108, 406, 0, 0, 2, 0),
(109, 371, 0, 0, 1, 2),
(110, 376, 0, 0, 0, 2),
(111, 413, 0, 0, 0, 2),
(112, 377, 0, 0, 1, 2),
(113, 382, 0, 0, 0, 2),
(114, 383, 0, 0, 1, 2),
(115, 388, 0, 0, 0, 2),
(116, 389, 0, 0, 2, 0),
(117, 395, 0, 0, 1, 2),
(118, 401, 0, 0, 2, 1),
(119, 407, 0, 0, 2, 1),
(120, 412, 0, 0, 0, 2),
(121, 414, 0, 0, 3, 0),
(122, 415, 0, 0, 3, 0),
(123, 416, 0, 0, 3, 1),
(124, 417, 0, 0, 3, 1),
(125, 418, 0, 0, 3, 0),
(126, 419, 0, 0, 3, 0),
(127, 420, 0, 0, 3, 1),
(128, 421, 0, 0, 3, 0),
(129, 422, 0, 0, 1, 3),
(130, 423, 0, 0, 0, 3),
(131, 424, 0, 0, 3, 0),
(132, 425, 0, 0, 2, 3),
(133, 426, 0, 0, 1, 4),
(134, 427, 0, 0, 4, 1),
(135, 428, 0, 0, 5, 3),
(136, 480, 0, 0, 0, 0),
(137, 456, 0, 0, 0, 0);

-- --------------------------------------------------------

--
-- Table structure for table `seeds`
--

CREATE TABLE `seeds` (
  `seedId` int(11) NOT NULL,
  `groupId` int(11) NOT NULL,
  `seedNr` int(11) NOT NULL,
  `seedName` text,
  `ancestorMatchId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `seeds`
--

INSERT INTO `seeds` (`seedId`, `groupId`, `seedNr`, `seedName`, `ancestorMatchId`) VALUES
(160, 47, 1, 'Ronny McHammergeil', NULL),
(161, 47, 2, 'Seed #2', NULL),
(162, 47, 3, 'Seed #3', NULL),
(163, 47, 4, 'Seed #4', NULL),
(164, 48, 5, 'M3tl3r', NULL),
(165, 48, 6, 'Toddler', NULL),
(166, 48, 7, 'Seed #7', NULL),
(167, 48, 8, 'Seed #8', NULL),
(168, 49, 9, 'Seed #9', NULL),
(169, 49, 10, 'Roter Samu', NULL),
(170, 49, 11, 'Seed #11', NULL),
(171, 49, 12, 'Seed #12', NULL),
(172, 50, 13, 'Eisentod', NULL),
(173, 50, 14, 'Fritto', NULL),
(174, 50, 15, 'Seed #15', NULL),
(175, 50, 16, 'Seed #16', NULL),
(176, 51, 17, 'Siracha', NULL),
(177, 51, 18, 'Piri Piri', NULL),
(178, 51, 19, 'Seed #19', NULL),
(179, 52, 20, 'The Lightning', NULL),
(180, 52, 21, 'MC Hammerschmidt', NULL),
(181, 52, 22, 'Seed #22', NULL),
(182, 53, 23, 'Erlero', NULL),
(183, 53, 24, 'The Fall Down Boy', NULL),
(184, 53, 25, 'Seed #25', NULL),
(185, 54, 26, 'Mirconan der Barbar', NULL),
(186, 54, 27, 'Krümelmännchen', NULL),
(187, 54, 28, 'Seed #28', NULL),
(188, 55, 1, 'Seed #1', NULL),
(189, 55, 2, 'The Fall Down Boy', NULL),
(190, 55, 3, 'Seed #3', NULL),
(191, 55, 4, 'Ralle Export', NULL),
(192, 56, 5, 'Seed #5', NULL),
(193, 56, 6, 'Seed #6', NULL),
(194, 56, 7, 'Captain Europe', NULL),
(195, 56, 8, 'Piri Piri', NULL),
(196, 57, 9, 'Seed #9', NULL),
(197, 57, 10, 'The Bullett', NULL),
(198, 57, 11, 'Seed #11', NULL),
(199, 57, 12, 'Wine Mania', NULL),
(200, 58, 13, 'Seed #13', NULL),
(201, 58, 14, 'Seed #14', NULL),
(202, 58, 15, 'Hawkcore', NULL),
(203, 58, 16, 'Der Unendliche', NULL),
(204, 59, 17, 'Sherrif', NULL),
(205, 59, 18, 'Regenbogenfo', NULL),
(206, 59, 19, 'Seed #19', NULL),
(207, 59, 20, 'Seed #20', NULL),
(208, 60, 21, 'Coach-Atze 69', NULL),
(209, 60, 22, 'Seed #22', NULL),
(210, 60, 23, 'Ziese', NULL),
(211, 60, 24, 'Seed #24', NULL),
(212, 61, 25, 'Seed #25', NULL),
(213, 61, 26, 'Total Eclipse of the Dart', NULL),
(214, 61, 27, 'Seed #27', NULL),
(215, 61, 28, 'The Smoking Rüdiger', NULL),
(216, 62, 29, 'The Caller', NULL),
(217, 62, 30, 'Seed #30', NULL),
(218, 62, 31, 'HansKlausFranz', NULL),
(219, 62, 32, 'Seed #32', NULL),
(264, 70, 0, 'Please Run Script', NULL),
(265, 70, 1, 'Please Run Script', NULL),
(266, 70, 2, 'Please Run Script', NULL),
(267, 70, 3, 'Please Run Script', NULL),
(268, 70, 4, 'Please Run Script', NULL),
(269, 70, 5, 'Please Run Script', NULL),
(270, 70, 6, 'Please Run Script', NULL),
(271, 70, 7, 'Please Run Script', NULL),
(272, 70, 8, 'Please Run Script', NULL),
(273, 70, 9, 'Please Run Script', NULL),
(274, 70, 10, 'Please Run Script', NULL),
(275, 70, 11, 'Please Run Script', NULL),
(276, 70, 12, 'Please Run Script', NULL),
(277, 70, 13, 'Please Run Script', NULL),
(278, 70, 14, 'Please Run Script', NULL),
(279, 70, 15, 'Please Run Script', NULL),
(280, 71, 0, 'The Fall Down Boy | The Caller', NULL),
(281, 71, 1, 'Piri Piri | Total Eclipse of the Dart', NULL),
(282, 71, 2, 'Wine Mania | Coach-Atze 69', NULL),
(283, 71, 3, 'Hawkcore | Regenbogenfo', NULL),
(284, 71, 4, 'Sherrif | Der Unendliche', NULL),
(285, 71, 5, 'Ziese | The Bullett', NULL),
(286, 71, 6, 'The Smoking Rüdiger | Captain Europe', NULL),
(287, 71, 7, 'HansKlausFranz | Ralle Export', NULL),
(288, 72, 0, 'The Fall Down Boy | Piri Piri', 175),
(289, 72, 0, 'Wine Mania | Hawkcore', 176),
(290, 72, 0, 'Sherrif | Ziese', 177),
(291, 72, 0, 'The Smoking Rüdiger | HansKlausFranz', 178),
(292, 73, 0, 'Piri Piri | Wine Mania', 179),
(293, 73, 0, 'Ziese | The Smoking Rüdiger', 180),
(294, 74, 1, 'Seed #1', NULL),
(295, 74, 2, 'Ziese', NULL),
(296, 74, 3, 'Turning Tables', NULL),
(297, 74, 4, 'Seed #4', NULL),
(298, 75, 5, 'Siracha', NULL),
(299, 75, 6, 'Seed #6', NULL),
(300, 75, 7, 'Seed #7', NULL),
(301, 75, 8, 'The Fall Down Boy', NULL),
(302, 76, 9, 'Seed #9', NULL),
(303, 76, 10, 'Seed #10', NULL),
(304, 76, 11, 'M3tl3r', NULL),
(305, 76, 12, 'Ralle Export', NULL),
(306, 77, 13, 'Seed #13', NULL),
(307, 77, 14, 'Toddler', NULL),
(308, 77, 15, 'Seed #15', NULL),
(309, 77, 16, 'The Tower', NULL),
(310, 78, 17, 'Piri Piri', NULL),
(311, 78, 18, 'Seed #18', NULL),
(312, 78, 19, 'Zitterhand', NULL),
(314, 79, 20, 'Basstart', NULL),
(315, 79, 21, 'Krümelmännchen', NULL),
(316, 79, 22, 'Seed #22', NULL),
(318, 80, 23, 'Seed #23', NULL),
(319, 80, 24, 'Roter Samu', NULL),
(320, 80, 25, 'Coach-Atze 69', NULL),
(322, 81, 26, 'The Lightning', NULL),
(323, 81, 27, 'Seed #27', NULL),
(324, 81, 28, 'Ronny McHammergeil', NULL),
(338, 84, 0, 'Please Run Script', NULL),
(339, 84, 0, 'Please Run Script', NULL),
(490, 105, 0, 'Please Run Script', NULL),
(491, 105, 1, 'Please Run Script', NULL),
(492, 105, 2, 'Please Run Script', NULL),
(493, 105, 3, 'Please Run Script', NULL),
(494, 105, 4, 'Please Run Script', NULL),
(495, 105, 5, 'Please Run Script', NULL),
(496, 105, 6, 'Please Run Script', NULL),
(497, 105, 7, 'Please Run Script', NULL),
(498, 105, 8, 'Please Run Script', NULL),
(499, 105, 9, 'Please Run Script', NULL),
(500, 105, 10, 'Please Run Script', NULL),
(501, 105, 11, 'Please Run Script', NULL),
(502, 105, 12, 'Please Run Script', NULL),
(503, 105, 13, 'Please Run Script', NULL),
(504, 105, 14, 'Please Run Script', NULL),
(505, 105, 15, 'Please Run Script', NULL),
(506, 106, 0, 'Turning Tables | Ronny McHammergeil', NULL),
(507, 106, 1, 'The Fall Down Boy | Roter Samu', NULL),
(508, 106, 2, 'Ralle Export | Krümelmännchen', NULL),
(509, 106, 3, 'The Tower | Zitterhand', NULL),
(510, 106, 4, 'Piri Piri | Toddler', NULL),
(511, 106, 5, 'Basstart | M3tl3r', NULL),
(512, 106, 6, 'Coach-Atze 69 | Siracha', NULL),
(513, 106, 7, 'The Lightning | Ziese', NULL),
(514, 107, 0, 'Turning Tables | The Fall Down Boy', 422),
(515, 107, 0, 'Ralle Export | The Tower', 423),
(516, 107, 0, 'Piri Piri | Basstart', 424),
(517, 107, 0, 'Coach-Atze 69 | The Lightning', 425),
(518, 108, 0, 'The Fall Down Boy | The Tower', 426),
(519, 108, 0, 'Piri Piri | The Lightning', 427),
(526, 111, 0, 'Please Run Script', NULL),
(527, 111, 1, 'Please Run Script', NULL),
(528, 111, 2, 'Please Run Script', NULL),
(529, 111, 3, 'Please Run Script', NULL),
(530, 111, 4, 'Please Run Script', NULL),
(531, 111, 5, 'Please Run Script', NULL),
(532, 111, 6, 'Please Run Script', NULL),
(533, 111, 7, 'Please Run Script', NULL),
(534, 111, 8, 'Please Run Script', NULL),
(535, 111, 9, 'Please Run Script', NULL),
(536, 111, 10, 'Please Run Script', NULL),
(537, 111, 11, 'Please Run Script', NULL),
(538, 111, 12, 'Please Run Script', NULL),
(539, 111, 13, 'Please Run Script', NULL),
(540, 111, 14, 'Please Run Script', NULL),
(541, 111, 15, 'Please Run Script', NULL),
(542, 112, 0, 'Please Run Script', NULL),
(543, 112, 1, 'Please Run Script', NULL),
(544, 112, 2, 'Please Run Script', NULL),
(545, 112, 3, 'Please Run Script', NULL),
(546, 112, 4, 'Please Run Script', NULL),
(547, 112, 5, 'Please Run Script', NULL),
(548, 112, 6, 'Please Run Script', NULL),
(549, 112, 7, 'Please Run Script', NULL),
(550, 113, 0, 'Please Run Script', 488),
(551, 113, 0, 'Please Run Script', 489),
(552, 113, 0, 'Please Run Script', 490),
(553, 113, 0, 'Please Run Script', 491),
(554, 114, 0, 'Please Run Script', 492),
(555, 114, 0, 'Please Run Script', 493);

-- --------------------------------------------------------

--
-- Table structure for table `tournaments`
--

CREATE TABLE `tournaments` (
  `tournamentId` int(11) NOT NULL,
  `name` text,
  `starttime` datetime DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `tournaments`
--

INSERT INTO `tournaments` (`tournamentId`, `name`, `starttime`) VALUES
(2, 'Chemodarts Vol. 1', NULL),
(3, 'Chemodarts Vol. 2', '2022-11-27 14:00:00'),
(4, 'Chemodarts Vol. 3', '2023-04-02 12:00:00');

-- --------------------------------------------------------

--
-- Table structure for table `venues`
--

CREATE TABLE `venues` (
  `venueId` int(11) NOT NULL,
  `name` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

--
-- Dumping data for table `venues`
--

INSERT INTO `venues` (`venueId`, `name`) VALUES
(5, 'Board 1'),
(6, 'Board 2'),
(7, 'Board 3'),
(8, 'Board 4'),
(9, 'Board 5');

--
-- Indexes for dumped tables
--

--
-- Indexes for table `groups`
--
ALTER TABLE `groups`
  ADD PRIMARY KEY (`groupId`),
  ADD KEY `roundId` (`roundId`);

--
-- Indexes for table `map_round_venue`
--
ALTER TABLE `map_round_venue`
  ADD PRIMARY KEY (`rvMapId`),
  ADD KEY `roundId` (`roundId`),
  ADD KEY `venueId` (`venueId`);

--
-- Indexes for table `map_tournament_seed_player`
--
ALTER TABLE `map_tournament_seed_player`
  ADD PRIMARY KEY (`tpMapId`),
  ADD KEY `groupId` (`tournamentId`),
  ADD KEY `playerId` (`playerId`),
  ADD KEY `seedId` (`seedId`),
  ADD KEY `tournamentId` (`tournamentId`),
  ADD KEY `playerId_2` (`playerId`);

--
-- Indexes for table `matches`
--
ALTER TABLE `matches`
  ADD PRIMARY KEY (`matchId`),
  ADD UNIQUE KEY `matchId_2` (`matchId`),
  ADD KEY `matchId` (`matchId`),
  ADD KEY `player1Id` (`seed1Id`),
  ADD KEY `player2Id` (`seed2Id`),
  ADD KEY `groupId` (`groupId`),
  ADD KEY `venueId` (`venueId`);

--
-- Indexes for table `players`
--
ALTER TABLE `players`
  ADD PRIMARY KEY (`playerId`),
  ADD UNIQUE KEY `PlayerId` (`playerId`);

--
-- Indexes for table `rounds`
--
ALTER TABLE `rounds`
  ADD PRIMARY KEY (`roundId`),
  ADD KEY `r_tId` (`tournamentId`);

--
-- Indexes for table `scores`
--
ALTER TABLE `scores`
  ADD PRIMARY KEY (`scoreId`),
  ADD UNIQUE KEY `matchId_2` (`matchId`),
  ADD KEY `matchId` (`matchId`);

--
-- Indexes for table `seeds`
--
ALTER TABLE `seeds`
  ADD PRIMARY KEY (`seedId`),
  ADD KEY `groupId` (`groupId`),
  ADD KEY `ancesterMatchId` (`ancestorMatchId`);

--
-- Indexes for table `tournaments`
--
ALTER TABLE `tournaments`
  ADD PRIMARY KEY (`tournamentId`);

--
-- Indexes for table `venues`
--
ALTER TABLE `venues`
  ADD PRIMARY KEY (`venueId`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `groups`
--
ALTER TABLE `groups`
  MODIFY `groupId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=115;
--
-- AUTO_INCREMENT for table `map_round_venue`
--
ALTER TABLE `map_round_venue`
  MODIFY `rvMapId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=24;
--
-- AUTO_INCREMENT for table `map_tournament_seed_player`
--
ALTER TABLE `map_tournament_seed_player`
  MODIFY `tpMapId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=132;
--
-- AUTO_INCREMENT for table `matches`
--
ALTER TABLE `matches`
  MODIFY `matchId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=495;
--
-- AUTO_INCREMENT for table `players`
--
ALTER TABLE `players`
  MODIFY `playerId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=54;
--
-- AUTO_INCREMENT for table `rounds`
--
ALTER TABLE `rounds`
  MODIFY `roundId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;
--
-- AUTO_INCREMENT for table `scores`
--
ALTER TABLE `scores`
  MODIFY `scoreId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=138;
--
-- AUTO_INCREMENT for table `seeds`
--
ALTER TABLE `seeds`
  MODIFY `seedId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=556;
--
-- AUTO_INCREMENT for table `tournaments`
--
ALTER TABLE `tournaments`
  MODIFY `tournamentId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=5;
--
-- AUTO_INCREMENT for table `venues`
--
ALTER TABLE `venues`
  MODIFY `venueId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;
--
-- Constraints for dumped tables
--

--
-- Constraints for table `groups`
--
ALTER TABLE `groups`
  ADD CONSTRAINT `g_rId` FOREIGN KEY (`roundId`) REFERENCES `rounds` (`roundId`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `map_round_venue`
--
ALTER TABLE `map_round_venue`
  ADD CONSTRAINT `map_rv_rId` FOREIGN KEY (`roundId`) REFERENCES `rounds` (`roundId`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `map_rv_vId` FOREIGN KEY (`venueId`) REFERENCES `venues` (`venueId`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `map_tournament_seed_player`
--
ALTER TABLE `map_tournament_seed_player`
  ADD CONSTRAINT `map_tp_pId` FOREIGN KEY (`playerId`) REFERENCES `players` (`playerId`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `map_tp_tId` FOREIGN KEY (`tournamentId`) REFERENCES `tournaments` (`tournamentId`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `map_ts_sId` FOREIGN KEY (`seedId`) REFERENCES `seeds` (`seedId`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `matches`
--
ALTER TABLE `matches`
  ADD CONSTRAINT `m_gId` FOREIGN KEY (`groupId`) REFERENCES `groups` (`groupId`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `m_s1Id` FOREIGN KEY (`seed1Id`) REFERENCES `seeds` (`seedId`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `m_s2Id` FOREIGN KEY (`seed2Id`) REFERENCES `seeds` (`seedId`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `m_vId` FOREIGN KEY (`venueId`) REFERENCES `venues` (`venueId`) ON UPDATE CASCADE;

--
-- Constraints for table `rounds`
--
ALTER TABLE `rounds`
  ADD CONSTRAINT `r_tId` FOREIGN KEY (`tournamentId`) REFERENCES `tournaments` (`tournamentId`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `scores`
--
ALTER TABLE `scores`
  ADD CONSTRAINT `s_mId` FOREIGN KEY (`matchId`) REFERENCES `matches` (`matchId`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `seeds`
--
ALTER TABLE `seeds`
  ADD CONSTRAINT `s_amId` FOREIGN KEY (`ancestorMatchId`) REFERENCES `matches` (`matchId`) ON DELETE SET NULL ON UPDATE CASCADE,
  ADD CONSTRAINT `s_gId` FOREIGN KEY (`groupId`) REFERENCES `groups` (`groupId`) ON DELETE CASCADE ON UPDATE CASCADE;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
