<!DOCTYPE xsl:stylesheet>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="text" indent="no"/>
		
	<xsl:template match="/">
		<html>
			<head>
				<style>
					table, td, th {
					border: 1px solid black;
					border-collapse: collapse;
					padding: 5 5;
					}
					td {
					padding: 2 5;
					}
				</style>
			</head>
			<body>
				<xsl:apply-templates select="schedule"/>
			</body>
		</html>
		
	</xsl:template>

	<xsl:template match="schedule">
		<div>
			Расписание НИУ ВШЭ учебной группы ПИ-20-1
		</div>
		<div>
			<table>
				<xsl:apply-templates select="week"/>
			</table>
		</div>
	</xsl:template>
		
	<xsl:template match="week">
		<tr>
			<th colspan="5">
				<xsl:text>Расписание на неделю от </xsl:text>
				<xsl:value-of select="@date"/>
			</th>
		</tr>
		<tr>
			<th>День</th>
			<th>Время</th>
			<th>Предмет, тип занятия</th>
			<th>Преподаватель</th>
			<th>Аудитория</th>
		</tr>
		<xsl:apply-templates select="day"/>
	</xsl:template>

	<xsl:template match="day">
		<tr>
			<td>
				<xsl:value-of select="@date"/>
			</td>
			<td>
				<xsl:value-of select="lesson[0]/@date"/>
			</td>
			<td>
				<xsl:value-of select="@date"/>
			</td>
			<td>
				<xsl:value-of select="@date"/>
			</td>
			<td>
				<xsl:value-of select="@date"/>
			</td>
		</tr>

		
		<xsl:text>:
</xsl:text>
		<xsl:choose>
			<xsl:when test="count(*)>0"><xsl:apply-templates select="lesson"/></xsl:when>
            <xsl:otherwise>-
</xsl:otherwise>
        </xsl:choose>
		
		<xsl:text>
</xsl:text>
	</xsl:template>

	<xsl:template match="lesson">
		<xsl:value-of select="substring(@start,0,6)"/>-<xsl:value-of select="substring(@end,0,6)"/> | <xsl:value-of select="@name"/> (<xsl:value-of select="@lecturer"/><xsl:text>, </xsl:text>
		<xsl:choose>
			<xsl:when test="@type=lecture">лекция</xsl:when>
            <xsl:otherwise>семинар</xsl:otherwise>
        </xsl:choose>		
		<xsl:text>) - </xsl:text><xsl:value-of select="@aud"/>
		<xsl:text>
</xsl:text>
	</xsl:template>

</xsl:stylesheet>