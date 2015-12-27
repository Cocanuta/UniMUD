-- --------------------------------------------------------

--
-- Table structure for table `Characters`
--

CREATE TABLE IF NOT EXISTS `Characters` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(60) COLLATE latin1_general_ci NOT NULL,
  `HP` float NOT NULL,
  `Location` int(11) NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `Name` (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci AUTO_INCREMENT=1 ;

-- --------------------------------------------------------

--
-- Table structure for table `Users`
--

CREATE TABLE IF NOT EXISTS `Users` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `Name` varchar(60) COLLATE latin1_general_ci NOT NULL,
  `Password` varchar(120) COLLATE latin1_general_ci NOT NULL,
  `Email` varchar(120) COLLATE latin1_general_ci NOT NULL,
  `Characters` varchar(60) COLLATE latin1_general_ci NOT NULL,
  PRIMARY KEY (`ID`),
  KEY `Name` (`Name`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_general_ci AUTO_INCREMENT=1 ;
