-- zoeken naar alle bordspellen van 1 speler (als vb speler 2) => spellen in collectie

-- SELECT BordspelGebruiker.* , '' as 'SplitCol', Bordspel.*
SELECT Bordspel.naam
FROM ProjectBGG.BordspelGebruiker
INNER JOIN ProjectBGG.Bordspel ON BordspelGebruiker.bordspelId = Bordspel.id 
WHERE gebruikerId = 2
ORDER BY naam;

-- zoeken naar alle bordspellen die NIET in collectie zitten van 1 speler (als vb speler 2) maar wel bestaan in databank

SELECT naam
FROM ProjectBGG.Bordspel 
WHERE id  NOT IN
	(SELECT bordspelId
		FROM ProjectBGG.BordspelGebruiker
			WHERE gebruikerId = 2)
ORDER BY Bordspel.naam;

-- zoeken bordspel op ID

SELECT * 
FROM ProjectBGG.Bordspel 
INNER JOIN ProjectBGG.BordspelPersoon ON Bordspel.id = BordspelPersoon.bordspelId 
INNER JOIN ProjectBGG.BordspelUitgeverij ON Bordspel.id = BordspelUitgeverij.bordspelId
INNER JOIN ProjectBGG.Uitgeverij ON BordspelUitgeverij.uitgeverijId = Uitgeverij.id
INNER JOIN ProjectBGG.Persoon ON BordspelPersoon.persoonId = Persoon.id
INNER JOIN ProjectBGG.Rol ON BordspelPersoon.rolId = Rol.id
WHERE Bordspel.id = 11

-- alle uitgeverijen

SELECT *
FROM ProjectBGG.Uitgeverij
ORDER BY naam;

-- alle personen die geen uitgeverij zijn

SELECT *
FROM ProjectBGG.Persoon
WHERE voornaam != 'Uitgeverij'
ORDER BY naam;

INSERT INTO [ProjectBGG].[Persoon] 
	VALUES ('Bauza','Antoine','Antoine Bauza (born 1978 )is the popular designer of several award-winning games, 
	including 7 Wonders and Hanabi (which won the 2013 Spiel des Jahres prize). Even before the success of 7 Wonders, 
	Antoine was well-known for his many other games, including Ghost Stories and Tokaido.',3);