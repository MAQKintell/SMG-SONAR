<xsl:stylesheet version="1.0"
    xmlns="urn:schemas-microsoft-com:office:spreadsheet"
    xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
 xmlns:msxsl="urn:schemas-microsoft-com:xslt"
 xmlns:user="urn:my-scripts"
 xmlns:o="urn:schemas-microsoft-com:office:office"
 xmlns:x="urn:schemas-microsoft-com:office:excel"
 xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet" > 
 
 <xsl:template match="/">
	  <Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet"
    xmlns:o="urn:schemas-microsoft-com:office:office"
    xmlns:x="urn:schemas-microsoft-com:office:excel"
    xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet"
    xmlns:html="http://www.w3.org/TR/REC-html40">
    </Workbook>
</xsl:template>
     
  
<xsl:template match="Report">

    <xsl:apply-templates/>
  <xsl:apply-templates select="Header" />
  <xsl:apply-templates select="Data" />
</xsl:template>

<xsl:template match="/*">
  <Worksheet>
  <xsl:attribute name="ss:Name">
  <xsl:value-of select="local-name(/*/*)"/>
  </xsl:attribute>
    <xsl:apply-templates select="Header"/>
    <xsl:apply-templates select="Data"/>
  </Worksheet>
</xsl:template>

<xsl:template match="Header">
	<!--<xsl:apply-templates select="Tittle"/>-->
  <xsl:apply-templates select="Attributes"/>
</xsl:template>

<xsl:template match="Tittle">
			<xsl:value-of select="."/>
</xsl:template>

<xsl:template match="Attributes">
	<table>
	<xsl:for-each select="Attribute">
		<Row>
			<Cell>
    		<xsl:value-of select="@name"/>:
    	</Cell>
    	<Cell>
    		<xsl:value-of select="."/>
    	</Cell>
    </Row>
  </xsl:for-each>
  </table>
</xsl:template>

<xsl:template match="Data">
	<table style="background-color:#ff0000" width="100%">
  <xsl:apply-templates select="DataHeader" />
  <xsl:apply-templates select="DataRows" />
  </table>
</xsl:template>


<xsl:template match="DataHeader">
	<Row>
		<xsl:for-each select="HeaderColumn">
    	<Cell>
    		<xsl:value-of select="@name"/>
    	</Cell>
  	</xsl:for-each>
  </Row>
</xsl:template>

 <xsl:template match="DataRows">
  <xsl:for-each select="Row">
  <Row>
	  <xsl:for-each select="RowColumn">
		  <Cell>
			  <xsl:value-of select="."/>
		  </Cell>
	  </xsl:for-each>
  </Row>
  </xsl:for-each>
</xsl:template>
</xsl:stylesheet>
