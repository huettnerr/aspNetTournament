-- phpMyAdmin SQL Dump
-- version 4.6.6
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1:52827
-- Generation Time: Sep 19, 2023 at 05:38 AM
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
(117, 14, 'Row Zero'),
(118, 14, 'Backstage'),
(119, 14, 'Chemoklo'),
(120, 14, 'Dance'),
(121, 14, 'Einlass'),
(122, 14, 'Freisitz'),
(123, 14, 'Garderobe'),
(124, 14, 'Tresen'),
(133, 15, 'Stufe der besten 16'),
(134, 15, 'Viertelfinale'),
(135, 15, 'Halbfinale'),
(136, 15, 'Finale'),
(137, 16, 'A'),
(138, 16, 'B'),
(139, 16, 'C'),
(140, 16, 'D'),
(141, 16, 'E'),
(142, 16, 'F'),
(143, 16, 'G'),
(144, 16, 'H'),
(154, 17, 'Viertelfinale'),
(155, 17, 'Halbfinale'),
(156, 17, 'Finale');

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
(8, 7, 5),
(9, 7, 6),
(10, 7, 7),
(11, 7, 8),
(12, 7, 9),
(14, 8, 5),
(15, 8, 6),
(16, 8, 7),
(17, 8, 8),
(18, 8, 9),
(19, 11, 5),
(20, 11, 6),
(21, 11, 7),
(22, 11, 8),
(23, 11, 9),
(24, 14, 5),
(25, 14, 6),
(26, 14, 7),
(27, 14, 8),
(28, 14, 9),
(29, 15, 5),
(30, 15, 6),
(31, 15, 7),
(32, 15, 8);

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
(131, 4, 324, 46, 0, 0),
(140, 5, 564, 11, 0, 0),
(141, 5, 565, 41, 0, 0),
(142, 5, 566, 60, 0, 0),
(143, 5, 567, 59, 0, 0),
(144, 5, 568, 57, 0, 0),
(145, 5, 569, 47, 0, 0),
(146, 5, 570, 42, 0, 0),
(147, 5, 571, 27, 0, 0),
(148, 5, 572, 61, 0, 0),
(149, 5, 573, 58, 0, 0),
(150, 5, 574, 24, 0, 0),
(151, 5, 575, 63, 0, 0),
(152, 5, 576, 7, 0, 0),
(153, 5, 577, 10, 0, 0),
(154, 5, 578, 54, 0, 0),
(156, 5, 580, 50, 0, 0),
(157, 5, 581, 44, 0, 0),
(158, 5, 582, 26, 0, 0),
(160, 5, 584, 39, 0, 0),
(161, 5, 585, 56, 0, 0),
(162, 5, 586, 8, 0, 0),
(164, 5, 588, 62, 0, 0),
(165, 5, 589, 22, 0, 0),
(166, 5, 590, 9, 0, 0),
(168, 5, 592, 37, 0, 0),
(169, 5, 593, 55, 0, 0),
(170, 5, 594, 33, 0, 0),
(171, 2, 686, 48, 0, 0),
(172, 2, 687, 21, 0, 0),
(173, 2, 688, 64, 0, 0),
(174, 2, 689, 25, 0, 0),
(175, 2, 690, 14, 0, 0),
(176, 2, 691, 13, 0, 0),
(177, 2, 692, 27, 0, 0),
(178, 2, 693, 65, 0, 0),
(179, 2, 694, 7, 0, 0),
(180, 2, 695, 26, 0, 0),
(181, 2, 696, 17, 0, 0),
(182, 2, 697, 34, 0, 0),
(183, 2, 698, 50, 0, 0),
(184, 2, 699, 43, 0, 0),
(185, 2, 700, 8, 0, 0),
(187, 2, 702, 66, 0, 0),
(188, 2, 703, 67, 0, 0),
(189, 2, 704, 24, 0, 0),
(190, 2, 705, 68, 0, 0),
(191, 2, 706, 33, 0, 0),
(192, 2, 707, 69, 0, 0),
(193, 2, 708, 16, 0, 0),
(194, 2, 709, 70, 0, 0),
(195, 2, 710, 71, 0, 0),
(196, 2, 711, 23, 0, 0),
(197, 2, 712, 20, 0, 0),
(198, 2, 713, 9, 0, 0);

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
(573, 1, 117, 564, 565, 'Finished', NULL, NULL, NULL),
(574, 1, 117, 566, 567, 'Finished', NULL, NULL, NULL),
(575, 3, 117, 564, 566, 'Finished', NULL, NULL, NULL),
(576, 3, 117, 565, 567, 'Finished', NULL, NULL, NULL),
(577, 5, 117, 564, 567, 'Finished', NULL, NULL, NULL),
(578, 5, 117, 565, 566, 'Finished', NULL, NULL, NULL),
(579, 1, 118, 568, 569, 'Finished', NULL, NULL, NULL),
(580, 1, 118, 570, 571, 'Finished', NULL, NULL, NULL),
(581, 3, 118, 568, 570, 'Finished', NULL, NULL, NULL),
(582, 3, 118, 569, 571, 'Finished', NULL, NULL, NULL),
(583, 5, 118, 568, 571, 'Finished', NULL, NULL, NULL),
(584, 5, 118, 569, 570, 'Finished', NULL, NULL, NULL),
(585, 1, 119, 572, 573, 'Finished', NULL, NULL, NULL),
(586, 1, 119, 574, 575, 'Finished', NULL, NULL, NULL),
(587, 3, 119, 572, 574, 'Finished', NULL, NULL, NULL),
(588, 3, 119, 573, 575, 'Finished', NULL, NULL, NULL),
(589, 5, 119, 572, 575, 'Finished', NULL, NULL, NULL),
(590, 5, 119, 573, 574, 'Finished', NULL, NULL, NULL),
(591, 0, 120, 576, 577, 'Finished', NULL, NULL, NULL),
(592, 1, 120, 577, 578, 'Finished', NULL, NULL, NULL),
(593, 2, 120, 576, 578, 'Finished', NULL, NULL, NULL),
(594, 3, 120, 578, 577, 'Finished', NULL, NULL, NULL),
(595, 4, 120, 578, 576, 'Finished', NULL, NULL, NULL),
(596, 5, 120, 577, 576, 'Finished', NULL, NULL, NULL),
(597, 0, 121, 580, 581, 'Finished', NULL, NULL, NULL),
(598, 1, 121, 581, 582, 'Finished', NULL, NULL, NULL),
(599, 2, 121, 580, 582, 'Finished', NULL, NULL, NULL),
(600, 3, 121, 582, 581, 'Finished', NULL, NULL, NULL),
(601, 4, 121, 582, 580, 'Finished', NULL, NULL, NULL),
(602, 5, 121, 581, 580, 'Finished', NULL, NULL, NULL),
(603, 0, 122, 584, 585, 'Finished', NULL, NULL, NULL),
(604, 1, 122, 585, 586, 'Finished', NULL, NULL, NULL),
(605, 2, 122, 584, 586, 'Finished', NULL, NULL, NULL),
(606, 3, 122, 586, 585, 'Finished', NULL, NULL, NULL),
(607, 4, 122, 586, 584, 'Finished', NULL, NULL, NULL),
(608, 5, 122, 585, 584, 'Finished', NULL, NULL, NULL),
(609, 0, 123, 588, 589, 'Finished', NULL, NULL, NULL),
(610, 1, 123, 589, 590, 'Finished', NULL, NULL, NULL),
(611, 2, 123, 588, 590, 'Finished', NULL, NULL, NULL),
(612, 3, 123, 590, 589, 'Finished', NULL, NULL, NULL),
(613, 4, 123, 590, 588, 'Finished', NULL, NULL, NULL),
(614, 5, 123, 589, 588, 'Finished', NULL, NULL, NULL),
(615, 0, 124, 592, 593, 'Finished', NULL, NULL, NULL),
(616, 1, 124, 593, 594, 'Finished', NULL, NULL, NULL),
(617, 2, 124, 592, 594, 'Finished', NULL, NULL, NULL),
(618, 3, 124, 594, 593, 'Finished', NULL, NULL, NULL),
(619, 4, 124, 594, 592, 'Finished', NULL, NULL, NULL),
(620, 5, 124, 593, 592, 'Finished', NULL, NULL, NULL),
(621, 0, 133, 565, 593, 'Finished', NULL, NULL, NULL),
(622, 1, 133, 568, 589, 'Finished', NULL, NULL, NULL),
(623, 2, 133, 575, 585, 'Finished', NULL, NULL, NULL),
(624, 3, 133, 578, 581, 'Finished', NULL, NULL, NULL),
(625, 4, 133, 580, 576, 'Finished', NULL, NULL, NULL),
(626, 5, 133, 584, 573, 'Finished', NULL, NULL, NULL),
(627, 6, 133, 588, 569, 'Finished', NULL, NULL, NULL),
(628, 7, 133, 592, 567, 'Finished', NULL, NULL, NULL),
(629, 0, 134, 565, 568, 'Finished', NULL, NULL, NULL),
(630, 1, 134, 575, 581, 'Finished', NULL, NULL, NULL),
(631, 2, 134, 576, 573, 'Finished', NULL, NULL, NULL),
(632, 3, 134, 569, 592, 'Finished', NULL, NULL, NULL),
(633, 0, 135, 565, 575, 'Finished', NULL, NULL, NULL),
(634, 1, 135, 573, 592, 'Finished', NULL, NULL, NULL),
(635, 0, 136, 575, 592, 'Finished', NULL, NULL, NULL),
(717, 1, 137, 686, 687, 'Finished', NULL, NULL, NULL),
(718, 1, 137, 688, 689, 'Finished', NULL, NULL, NULL),
(719, 3, 137, 686, 688, 'Finished', NULL, NULL, NULL),
(720, 3, 137, 687, 689, 'Finished', NULL, NULL, NULL),
(721, 5, 137, 686, 689, 'Finished', NULL, NULL, NULL),
(722, 5, 137, 687, 688, 'Finished', NULL, NULL, NULL),
(723, 1, 138, 690, 691, 'Finished', NULL, NULL, NULL),
(724, 1, 138, 692, 693, 'Finished', NULL, NULL, NULL),
(725, 3, 138, 690, 692, 'Finished', NULL, NULL, NULL),
(726, 3, 138, 691, 693, 'Finished', NULL, NULL, NULL),
(727, 5, 138, 690, 693, 'Finished', NULL, NULL, NULL),
(728, 5, 138, 691, 692, 'Finished', NULL, NULL, NULL),
(729, 1, 139, 694, 695, 'Finished', NULL, NULL, NULL),
(730, 1, 139, 696, 697, 'Finished', NULL, NULL, NULL),
(731, 3, 139, 694, 696, 'Finished', NULL, NULL, NULL),
(732, 3, 139, 695, 697, 'Finished', NULL, NULL, NULL),
(733, 5, 139, 694, 697, 'Finished', NULL, NULL, NULL),
(734, 5, 139, 695, 696, 'Finished', NULL, NULL, NULL),
(735, 0, 140, 698, 699, 'Finished', NULL, NULL, NULL),
(736, 1, 140, 699, 700, 'Finished', NULL, NULL, NULL),
(737, 2, 140, 698, 700, 'Finished', NULL, NULL, NULL),
(738, 3, 140, 700, 699, 'Finished', NULL, NULL, NULL),
(739, 4, 140, 700, 698, 'Finished', NULL, NULL, NULL),
(740, 5, 140, 699, 698, 'Finished', NULL, NULL, NULL),
(741, 0, 141, 702, 703, 'Finished', NULL, NULL, NULL),
(742, 1, 141, 703, 704, 'Finished', NULL, NULL, NULL),
(743, 2, 141, 702, 704, 'Finished', NULL, NULL, NULL),
(744, 3, 141, 704, 703, 'Finished', NULL, NULL, NULL),
(745, 4, 141, 704, 702, 'Finished', NULL, NULL, NULL),
(746, 5, 141, 703, 702, 'Finished', NULL, NULL, NULL),
(747, 0, 142, 705, 706, 'Finished', NULL, NULL, NULL),
(748, 1, 142, 706, 707, 'Finished', NULL, NULL, NULL),
(749, 2, 142, 705, 707, 'Finished', NULL, NULL, NULL),
(750, 3, 142, 707, 706, 'Finished', NULL, NULL, NULL),
(751, 4, 142, 707, 705, 'Finished', NULL, NULL, NULL),
(752, 5, 142, 706, 705, 'Finished', NULL, NULL, NULL),
(753, 0, 143, 708, 709, 'Finished', NULL, NULL, NULL),
(754, 1, 143, 709, 710, 'Finished', NULL, NULL, NULL),
(755, 2, 143, 708, 710, 'Finished', NULL, NULL, NULL),
(756, 3, 143, 710, 709, 'Finished', NULL, NULL, NULL),
(757, 4, 143, 710, 708, 'Finished', NULL, NULL, NULL),
(758, 5, 143, 709, 708, 'Finished', NULL, NULL, NULL),
(759, 0, 144, 711, 712, 'Finished', NULL, NULL, NULL),
(760, 1, 144, 712, 713, 'Finished', NULL, NULL, NULL),
(761, 2, 144, 711, 713, 'Finished', NULL, NULL, NULL),
(762, 3, 144, 713, 712, 'Finished', NULL, NULL, NULL),
(763, 4, 144, 713, 711, 'Finished', NULL, NULL, NULL),
(764, 5, 144, 712, 711, 'Finished', NULL, NULL, NULL),
(765, 0, 154, 688, 711, 'Finished', NULL, NULL, NULL),
(766, 1, 154, 694, 705, 'Finished', NULL, NULL, NULL),
(767, 2, 154, 690, 708, 'Finished', NULL, NULL, NULL),
(768, 3, 154, 698, 702, 'Finished', NULL, NULL, NULL),
(769, 0, 155, 711, 705, 'Finished', NULL, NULL, NULL),
(770, 1, 155, 690, 702, 'Finished', NULL, NULL, NULL),
(771, 0, 156, 705, 702, 'Finished', NULL, NULL, NULL);

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
(41, 'Leopold', 'Piri Piri', NULL, 'Kontra K', 'Erfolg ist kein Glück'),
(42, 'Martin', 'Erlero', NULL, 'Backstreet Boys', 'Everybody'),
(43, 'Philipp', 'The Bullett', NULL, '1. FC Nürnberg', 'Die Legende lebt'),
(44, 'Moritz', 'Wine Mania', NULL, 'Sepultura', 'Bloody Roots'),
(45, 'Vadim', 'Strahlemann', NULL, '\"Serienintro\"', 'Sailermoon'),
(46, 'Alex Heinze', 'Ronny McHammergeil', NULL, 'Elsterglanz', 'Kaputtschaahn'),
(47, 'Andreas Gregor', 'The Tower', NULL, 'Megaton Sword', 'Cowards Remain'),
(48, 'Manu', 'The Flying Koggsman', NULL, 'Bon Jovi', 'Raise your Hands'),
(49, 'Chris', 'M3tl3r', NULL, 'Dicks on Fire', 'Superbad Motherfucker'),
(50, 'Marcel Davideit', 'The Lightning', NULL, 'MOP', 'Ante Up'),
(51, 'Jakob Piribauer', 'Siracha', NULL, NULL, NULL),
(52, 'Toni', 'Fit Tony', NULL, 'Broilers', 'Küss meinen Ring'),
(53, 'Pete', 'Grind Peter', NULL, 'Municipal Waste', 'Born to Party'),
(54, 'Max', 'Der Verkehrte', NULL, 'Thomas & Friends', 'Thomas Theme'),
(55, 'Robert', 'The Omen', NULL, 'Magic Affair', 'Omen III'),
(56, 'Michel', 'Sippi', NULL, 'Mehnersmoss', 'Bir'),
(57, 'Kotze', 'Dartagnion', NULL, 'Oxo86', 'Manchmal'),
(58, 'Martin', 'Martin', NULL, 'Martin', 'Martin'),
(59, 'Christoph', 'Klaus', NULL, NULL, NULL),
(60, 'Robin', 'Fritz Berger', NULL, 'Motorhead', 'Sympathy'),
(61, 'Raiko', 'Dagobert Dart', NULL, 'Angerfist', 'Hoax'),
(62, 'Lukas', 'Pink Flamingo', NULL, 'Angerfist', 'Bodybag'),
(63, 'Simon', 'Ronny\'s größter Fan', NULL, 'Jpetersen', 'Ho Now'),
(64, 'Manuel Sieber', NULL, NULL, NULL, NULL),
(65, 'Chrischi', NULL, NULL, NULL, NULL),
(66, 'Mike Kautschat', 'Magic Mike', NULL, NULL, NULL),
(67, 'Wendelin Rabe', NULL, NULL, NULL, NULL),
(68, 'Rico Lubertsch', 'The Shadow', NULL, NULL, NULL),
(69, 'Adrian', NULL, NULL, NULL, NULL),
(70, 'Marius Baumgürtel', NULL, NULL, NULL, NULL),
(71, 'Milbe', NULL, NULL, NULL, NULL);

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
(6, 3, 'Vorgeplänkel', 'RoundRobin', 'LegsOnly'),
(7, 3, 'On Stage!', 'SingleKo', 'LegsOnly'),
(8, 4, 'Vorbands', 'RoundRobin', 'LegsOnly'),
(11, 4, 'On Stage!', 'SingleKo', 'LegsOnly'),
(14, 5, 'Verortung', 'RoundRobin', 'LegsOnly'),
(15, 5, 'On Stage!', 'SingleKo', 'LegsOnly'),
(16, 2, 'Vorrunde', 'RoundRobin', 'LegsOnly'),
(17, 2, 'On Stage!', 'SingleKo', 'LegsOnly');

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
(141, 573, 0, 0, 0, 2),
(142, 574, 0, 0, 0, 2),
(143, 579, 0, 0, 2, 0),
(144, 580, 0, 0, 2, 1),
(145, 585, 0, 0, 0, 2),
(146, 597, 0, 0, 2, 1),
(147, 591, 0, 0, 2, 1),
(148, 603, 0, 0, 1, 2),
(149, 609, 0, 0, 2, 0),
(150, 615, 0, 0, 2, 1),
(151, 586, 0, 0, 1, 2),
(152, 592, 0, 0, 1, 2),
(153, 598, 0, 0, 2, 1),
(157, 616, 0, 0, 2, 0),
(158, 593, 0, 0, 0, 2),
(159, 610, 0, 0, 2, 0),
(160, 604, 0, 0, 2, 1),
(161, 599, 0, 0, 2, 1),
(162, 575, 0, 0, 0, 2),
(163, 617, 0, 0, 2, 0),
(164, 611, 0, 0, 2, 0),
(165, 576, 0, 0, 2, 0),
(166, 605, 0, 0, 2, 0),
(167, 581, 0, 0, 2, 1),
(168, 582, 0, 0, 2, 0),
(169, 587, 0, 0, 0, 2),
(170, 588, 0, 0, 1, 2),
(171, 594, 0, 0, 2, 0),
(172, 600, 0, 0, 1, 2),
(173, 606, 0, 0, 2, 0),
(174, 612, 0, 0, 2, 1),
(175, 618, 0, 0, 0, 2),
(176, 595, 0, 0, 2, 0),
(177, 584, 0, 0, 2, 1),
(178, 601, 0, 0, 2, 1),
(179, 577, 0, 0, 2, 1),
(180, 607, 0, 0, 0, 2),
(181, 578, 0, 0, 2, 1),
(182, 619, 0, 0, 0, 2),
(183, 583, 0, 0, 2, 0),
(184, 589, 0, 0, 0, 2),
(185, 613, 0, 0, 0, 2),
(186, 590, 0, 0, 2, 0),
(187, 596, 0, 0, 0, 2),
(188, 602, 0, 0, 1, 2),
(189, 608, 0, 0, 0, 2),
(190, 614, 0, 0, 0, 2),
(191, 620, 0, 0, 2, 1),
(192, 621, 0, 0, 3, 0),
(193, 622, 0, 0, 3, 0),
(194, 623, 0, 0, 3, 0),
(195, 624, 0, 0, 1, 3),
(196, 626, 0, 0, 0, 3),
(197, 625, 0, 0, 0, 3),
(198, 627, 0, 0, 1, 3),
(199, 628, 0, 0, 3, 0),
(200, 629, 0, 0, 3, 2),
(201, 630, 0, 0, 3, 0),
(202, 631, 0, 0, 1, 3),
(203, 632, 0, 0, 1, 3),
(204, 633, 0, 0, 2, 4),
(205, 634, 0, 0, 1, 4),
(206, 635, 0, 0, 1, 5),
(263, 765, 0, 0, 1, 3),
(264, 766, 0, 0, 1, 3),
(265, 767, 0, 0, 3, 1),
(266, 768, 0, 0, 2, 3),
(267, 769, 0, 0, 1, 3),
(268, 770, 0, 0, 2, 3),
(269, 771, 0, 0, 3, 4),
(270, 717, 0, 0, 0, 2),
(271, 718, 0, 0, 2, 0),
(272, 719, 0, 0, 0, 2),
(273, 720, 0, 0, 2, 1),
(274, 721, 0, 0, 2, 0),
(275, 722, 0, 0, 0, 2),
(276, 723, 0, 0, 2, 0),
(277, 724, 0, 0, 2, 1),
(278, 725, 0, 0, 2, 0),
(279, 726, 0, 0, 2, 0),
(280, 727, 0, 0, 2, 0),
(281, 728, 0, 0, 2, 1),
(282, 729, 0, 0, 2, 0),
(283, 730, 0, 0, 2, 1),
(284, 731, 0, 0, 2, 0),
(285, 732, 0, 0, 2, 0),
(286, 733, 0, 0, 2, 0),
(287, 734, 0, 0, 2, 1),
(288, 735, 0, 0, 1, 2),
(289, 736, 0, 0, 2, 0),
(290, 737, 0, 0, 2, 0),
(291, 738, 0, 0, 0, 2),
(292, 739, 0, 0, 1, 2),
(293, 740, 0, 0, 0, 2),
(294, 741, 0, 0, 2, 0),
(295, 742, 0, 0, 2, 0),
(296, 743, 0, 0, 2, 0),
(297, 744, 0, 0, 1, 2),
(298, 745, 0, 0, 1, 2),
(299, 746, 0, 0, 0, 2),
(300, 747, 0, 0, 2, 0),
(301, 748, 0, 0, 2, 0),
(302, 749, 0, 0, 2, 0),
(303, 750, 0, 0, 0, 2),
(304, 751, 0, 0, 0, 2),
(305, 752, 0, 0, 0, 2),
(306, 753, 0, 0, 2, 1),
(307, 754, 0, 0, 2, 0),
(308, 755, 0, 0, 2, 0),
(309, 756, 0, 0, 2, 1),
(310, 757, 0, 0, 0, 1),
(311, 758, 0, 0, 1, 2),
(312, 759, 0, 0, 2, 0),
(313, 761, 0, 0, 2, 0),
(314, 760, 0, 0, 2, 0),
(315, 762, 0, 0, 1, 2),
(316, 763, 0, 0, 0, 2),
(317, 764, 0, 0, 2, 1);

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
(564, 117, 1, 'Seed #1', NULL),
(565, 117, 2, 'Piri Piri', NULL),
(566, 117, 3, 'Seed #3', NULL),
(567, 117, 4, 'Klaus', NULL),
(568, 118, 5, 'Dartagnion', NULL),
(569, 118, 6, 'The Tower', NULL),
(570, 118, 7, 'Seed #7', NULL),
(571, 118, 8, 'Seed #8', NULL),
(572, 119, 9, 'Seed #9', NULL),
(573, 119, 10, 'Martin', NULL),
(574, 119, 11, 'Seed #11', NULL),
(575, 119, 12, 'Ronny\'s größter Fan', NULL),
(576, 120, 13, 'Turning Tables', NULL),
(577, 120, 14, 'Seed #14', NULL),
(578, 120, 15, 'Der Verkehrte', NULL),
(580, 121, 17, 'The Lightning', NULL),
(581, 121, 18, 'Wine Mania', NULL),
(582, 121, 19, 'Seed #19', NULL),
(584, 122, 21, 'Total Eclipse of the Dart', NULL),
(585, 122, 22, 'Sippi', NULL),
(586, 122, 23, 'Seed #23', NULL),
(588, 123, 25, 'Pink Flamingo', NULL),
(589, 123, 26, 'Eisentod', NULL),
(590, 123, 27, 'Seed #27', NULL),
(592, 124, 29, 'The Fall Down Boy', NULL),
(593, 124, 30, 'The Omen', NULL),
(594, 124, 31, 'Seed #31', NULL),
(656, 133, 0, 'Please Run Script', NULL),
(657, 133, 1, 'Please Run Script', NULL),
(658, 133, 2, 'Please Run Script', NULL),
(659, 133, 3, 'Please Run Script', NULL),
(660, 133, 4, 'Please Run Script', NULL),
(661, 133, 5, 'Please Run Script', NULL),
(662, 133, 6, 'Please Run Script', NULL),
(663, 133, 7, 'Please Run Script', NULL),
(664, 133, 8, 'Please Run Script', NULL),
(665, 133, 9, 'Please Run Script', NULL),
(666, 133, 10, 'Please Run Script', NULL),
(667, 133, 11, 'Please Run Script', NULL),
(668, 133, 12, 'Please Run Script', NULL),
(669, 133, 13, 'Please Run Script', NULL),
(670, 133, 14, 'Please Run Script', NULL),
(671, 133, 15, 'Please Run Script', NULL),
(672, 134, 0, 'Piri Piri | The Omen', NULL),
(673, 134, 1, 'Dartagnion | Eisentod', NULL),
(674, 134, 2, 'Ronny\'s größter Fan | Sippi', NULL),
(675, 134, 3, 'Der Verkehrte | Wine Mania', NULL),
(676, 134, 4, 'The Lightning | Turning Tables', NULL),
(677, 134, 5, 'Total Eclipse of the Dart | Martin', NULL),
(678, 134, 6, 'Pink Flamingo | The Tower', NULL),
(679, 134, 7, 'The Fall Down Boy | Klaus', NULL),
(680, 135, 0, 'Piri Piri | Dartagnion', 629),
(681, 135, 0, 'Ronny\'s größter Fan | Wine Mania', 630),
(682, 135, 0, 'Turning Tables | Martin', 631),
(683, 135, 0, 'The Tower | The Fall Down Boy', 632),
(684, 136, 0, 'Piri Piri | Ronny\'s größter Fan', 633),
(685, 136, 0, 'Martin | The Fall Down Boy', 634),
(686, 137, 1, 'The Flying Koggsman', NULL),
(687, 137, 2, 'Seed #2', NULL),
(688, 137, 3, 'Seed #3', NULL),
(689, 137, 4, 'Seed #4', NULL),
(690, 138, 5, 'Ralle Export', NULL),
(691, 138, 6, 'Seed #6', NULL),
(692, 138, 7, 'Seed #7', NULL),
(693, 138, 8, 'Seed #8', NULL),
(694, 139, 9, 'Turning Tables', NULL),
(695, 139, 10, 'Seed #10', NULL),
(696, 139, 11, 'Seed #11', NULL),
(697, 139, 12, 'Seed #12', NULL),
(698, 140, 13, 'The Lightning', NULL),
(699, 140, 14, 'Seed #14', NULL),
(700, 140, 15, 'Seed #15', NULL),
(702, 141, 16, 'Magic Mike', NULL),
(703, 141, 17, 'Seed #17', NULL),
(704, 141, 18, 'Seed #18', NULL),
(705, 142, 19, 'The Shadow', NULL),
(706, 142, 20, 'Seed #20', NULL),
(707, 142, 21, 'Seed #21', NULL),
(708, 143, 22, 'Der Unendliche', NULL),
(709, 143, 23, 'Seed #23', NULL),
(710, 143, 24, 'Seed #24', NULL),
(711, 144, 25, 'Coach-Atze 69', NULL),
(712, 144, 26, 'Seed #26', NULL),
(713, 144, 27, 'Seed #27', NULL),
(756, 154, 0, 'Please Run Script', NULL),
(757, 154, 1, 'Please Run Script', NULL),
(758, 154, 2, 'Please Run Script', NULL),
(759, 154, 3, 'Please Run Script', NULL),
(760, 154, 4, 'Please Run Script', NULL),
(761, 154, 5, 'Please Run Script', NULL),
(762, 154, 6, 'Please Run Script', NULL),
(763, 154, 7, 'Please Run Script', NULL),
(764, 155, 0, 'Please Run Script', NULL),
(765, 155, 1, 'Please Run Script', NULL),
(766, 155, 2, 'Please Run Script', NULL),
(767, 155, 3, 'Please Run Script', NULL),
(768, 156, 0, 'Coach-Atze 69 | The Shadow', 769),
(769, 156, 0, 'Ralle Export | Magic Mike', 770);

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
(4, 'Chemodarts Vol. 3', '2023-04-02 12:00:00'),
(5, 'Chemodarts Vol. 4', '2023-09-17 12:00:00');

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
  MODIFY `groupId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=157;
--
-- AUTO_INCREMENT for table `map_round_venue`
--
ALTER TABLE `map_round_venue`
  MODIFY `rvMapId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=33;
--
-- AUTO_INCREMENT for table `map_tournament_seed_player`
--
ALTER TABLE `map_tournament_seed_player`
  MODIFY `tpMapId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=199;
--
-- AUTO_INCREMENT for table `matches`
--
ALTER TABLE `matches`
  MODIFY `matchId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=772;
--
-- AUTO_INCREMENT for table `players`
--
ALTER TABLE `players`
  MODIFY `playerId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=72;
--
-- AUTO_INCREMENT for table `rounds`
--
ALTER TABLE `rounds`
  MODIFY `roundId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=18;
--
-- AUTO_INCREMENT for table `scores`
--
ALTER TABLE `scores`
  MODIFY `scoreId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=318;
--
-- AUTO_INCREMENT for table `seeds`
--
ALTER TABLE `seeds`
  MODIFY `seedId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=770;
--
-- AUTO_INCREMENT for table `tournaments`
--
ALTER TABLE `tournaments`
  MODIFY `tournamentId` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;
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
