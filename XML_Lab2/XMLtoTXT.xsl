<!DOCTYPE xsl:stylesheet>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:output method="text" indent="no"/>
		
	<xsl:template match="/">
		<xsl:apply-templates select="schedule"/>
	</xsl:template>

	<xsl:template match="schedule">
		<xsl:text>Расписание НИУ ВШЭ учебной группы ПИ-20-1
</xsl:text>
		<xsl:apply-templates select="week"/>
	</xsl:template>
		
	<xsl:template match="week">
		<xsl:text>Расписание на неделю от </xsl:text>
		<xsl:value-of select="@date"/>
		<xsl:text>:
</xsl:text>
		<xsl:apply-templates select="day"/>
	</xsl:template>

	<xsl:template match="day">
		<xsl:value-of select="@date"/>
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