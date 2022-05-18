<xsl:stylesheet version="1.0"
  xmlns="urn:schemas-microsoft-com:office:spreadsheet"
  xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
  xmlns:msxsl="urn:schemas-microsoft-com:xslt"
  xmlns:user="urn:my-scripts"
  xmlns:o="urn:schemas-microsoft-com:office:office"
  xmlns:x="urn:schemas-microsoft-com:office:excel"
  xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet" >

  <xsl:output method="xml" indent="yes"/>
  <!--VARIABLES y PARAM Globales-->
  <!--Guarda el número de Atributos de la cabecera-->
  <xsl:param name="headerAttributesCount" select="count(//Header/HeaderAttributes/HeaderAttribute)"/>
  <!--Guarda el número de Columnas que se mostraran como datos-->
  <xsl:param name="headerColumnCount" select="count(//Data/DataHeader/HeaderColumn)"/>
  <!--"=Informe!R headerAttributesCount C2:R headerAttributesCount CheaderColumnCount, para realizar el autofilter. + 10 es el numero de columnas fijas" -->
  <xsl:variable name="rowFilter">=Informe!R<xsl:value-of select="$headerAttributesCount + 10"></xsl:value-of>C2:R<xsl:value-of select="$headerAttributesCount + 10"></xsl:value-of>C<xsl:value-of select="$headerColumnCount + 1"/></xsl:variable>
  <!--Para el autofilter R15C2:R15C53 -->
  <!-- R headerAttributesCount C2:R headerAttributesCount C headerColumnCount-->
  <xsl:variable name="rowAutoFilter">R<xsl:value-of select="$headerAttributesCount + 10"></xsl:value-of>C2:R<xsl:value-of select="$headerAttributesCount + 10"></xsl:value-of>C<xsl:value-of select="$headerColumnCount + 1"/></xsl:variable>

  <!--WORKBOOK-->
  <xsl:template match="/">
    <xsl:processing-instruction name="mso-application">progid="Excel.Sheet"</xsl:processing-instruction>
    <Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet"
    xmlns:o="urn:schemas-microsoft-com:office:office"
    xmlns:x="urn:schemas-microsoft-com:office:excel"
    xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet"
    xmlns:html="http://www.w3.org/TR/REC-html40">
      <!--DEFINICIÓN DE ESTILOS-->
      <Styles>
        <Style ss:ID="Default" ss:Name="Normal">
          <Alignment ss:Vertical="Bottom"/>
          <Borders/>
          <Font ss:FontName="Calibri" x:Family="Swiss" ss:Size="11" ss:Color="#000000"/>
          <Interior/>
          <NumberFormat/>
          <Protection/>
        </Style>
        <Style ss:ID="m28009584">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="2"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="2"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Trebuchet MS" x:Family="Swiss" ss:Size="11" ss:Bold="1"/>
        </Style>
        <Style ss:ID="m28024410">
          <Alignment ss:Horizontal="Center" ss:Vertical="Center"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="2"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="2"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="2"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="2"/>
          </Borders>
          <Font ss:FontName="Trebuchet MS" x:Family="Swiss" ss:Size="26"
           ss:Color="#000000"/>
        </Style>
        <Style ss:ID="m28024420">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="2"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="2"/>
          </Borders>
          <Font ss:FontName="Trebuchet MS" x:Family="Swiss" ss:Size="11" ss:Bold="1"/>
        </Style>
        <Style ss:ID="m28024430">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="2"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Trebuchet MS" x:Family="Swiss" ss:Size="11" ss:Bold="1"/>
        </Style>
        <Style ss:ID="cuadrado">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="2"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="2"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="2"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="2"/>
          </Borders>
          <Font ss:FontName="Trebuchet MS" x:Family="Swiss" ss:Size="11" ss:Bold="1"/>
        </Style>
        <Style ss:ID="s21">
          <Font ss:FontName="Calibri" x:Family="Swiss" ss:Size="11" ss:Color="#FFFFFF"/>
          <Interior ss:Color="#228B22" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s23">
          <Alignment ss:Horizontal="Center" ss:Vertical="Bottom"/>
          <Font ss:FontName="Trebuchet MS" x:Family="Swiss" ss:Size="24"
           ss:Color="#FFFFFF" ss:Bold="1"/>
          <Interior ss:Color="#228B22" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s24">
          <Alignment ss:Horizontal="Center" ss:Vertical="Bottom"/>
          <Font ss:FontName="Trebuchet MS" x:Family="Swiss" ss:Size="48"
           ss:Color="#000000"/>
        </Style>
        <Style ss:ID="s37">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="2"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="2"/>
          </Borders>
          <Font ss:FontName="Trebuchet MS" x:Family="Swiss" ss:Size="11" ss:Bold="1"/>
        </Style>
        <Style ss:ID="s42">
          <Alignment ss:Horizontal="Center" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="2"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="2"/>
          </Borders>
          <NumberFormat ss:Format="Short Date"/>
        </Style>
        <Style ss:ID="s43">
          <Font ss:FontName="Calibri" x:Family="Swiss" ss:Size="16" ss:Color="#000000"/>
        </Style>
        <Style ss:ID="s44">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="2"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
        </Style>
        <Style ss:ID="s45">
          <Font ss:FontName="Calibri" x:Family="Swiss" ss:Size="11" ss:Color="#FFFFFF"/>
        </Style>
        <Style ss:ID="s50">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="2"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
        </Style>
        <Style ss:ID="s55">
          <Alignment ss:Horizontal="Left" ss:Vertical="Bottom"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="2"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="2"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
        </Style>
        <Style ss:ID="s56">
          <Font ss:FontName="Calibri" x:Family="Swiss" ss:Size="11" ss:Color="#FFFFFF"/>
        </Style>
        <Style ss:ID="s57">
          <Alignment ss:Vertical="Center"/>
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Trebuchet MS" x:Family="Swiss" ss:Size="11"
           ss:Color="#FFFFFF" ss:Bold="1"/>
          <Interior ss:Color="#228B22" ss:Pattern="Solid"/>
        </Style>
        <Style ss:ID="s58">
          <Borders>
            <Border ss:Position="Bottom" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Left" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Right" ss:LineStyle="Continuous" ss:Weight="1"/>
            <Border ss:Position="Top" ss:LineStyle="Continuous" ss:Weight="1"/>
          </Borders>
          <Font ss:FontName="Trebuchet MS" x:Family="Swiss" ss:Color="#000000"/>
        </Style>
      </Styles>
      <!--Aplica la template del nodo principal-->
      <xsl:apply-templates select="Report"/>
    </Workbook>
  </xsl:template>

  <!--TEMPLATE Report-->
  <xsl:template match="Report">
    <!--Nombre del tab en ss:Name-->
    <Worksheet ss:Name="Informe">
      <!--Indica el rango que abarca el autofilter-->
      <Names>
        <NamedRange ss:Name="_FilterDatabase" ss:RefersTo="{$rowFilter}"
         ss:Hidden="1"/>
      </Names>

      <Table x:FullColumns="1"
           x:FullRows="1" ss:DefaultColumnWidth="60" ss:DefaultRowHeight="15">
        <Column ss:AutoFitWidth="0" ss:Width="13.5"/>
        <Column ss:Width="73.5" ss:Span="11"/>

        <!--Aplica las templates de datos de cabecera y los propios datos-->
        <xsl:apply-templates select="Header"/>
        <xsl:apply-templates select="Data"/>
      </Table>
      <!--Establece las propiedades del tab-->
      <WorksheetOptions xmlns="urn:schemas-microsoft-com:office:excel">
        <PageSetup>
          <Header x:Margin="0.3"/>
          <Footer x:Margin="0.3"/>
          <PageMargins x:Bottom="0.75" x:Left="0.7" x:Right="0.7" x:Top="0.75"/>
        </PageSetup>
        <Unsynced/>
        <Print>
          <ValidPrinterInfo/>
          <PaperSizeIndex>9</PaperSizeIndex>
          <HorizontalResolution>600</HorizontalResolution>
          <VerticalResolution>600</VerticalResolution>
        </Print>
        <Selected/>
        <Zoom>85</Zoom>
        <!--Hace que no salga la cuadricula-->
        <DoNotDisplayGridlines/>
        <!--Establece donde va el FreezePanel-->
        <FreezePanes/>
        <FrozenNoSplit/>
        <!--El freeze panel sera el numero de nodos atributo que exista mas 10 columnas que son fijas de titulos-->
        <SplitHorizontal>
          <xsl:copy-of select ="$headerAttributesCount + 10"/>
        </SplitHorizontal>
        <TopRowBottomPane>
          <xsl:copy-of select ="$headerAttributesCount + 10"/>
        </TopRowBottomPane>
        <ActivePane>2</ActivePane>
        <Panes>
          <Pane>
            <Number>3</Number>
          </Pane>
          <Pane>
            <Number>2</Number>
          </Pane>
        </Panes>
        <ProtectObjects>False</ProtectObjects>
        <ProtectScenarios>False</ProtectScenarios>
      </WorksheetOptions>
      <!--Establece el rango del autofilter, usando variable-->
      <AutoFilter x:Range="{$rowAutoFilter}"
      xmlns="urn:schemas-microsoft-com:office:excel">
      </AutoFilter>
    </Worksheet>
  </xsl:template>

  <!--TEMPLATE Header, titulo + información general del report-->
  <xsl:template match="Header">
    <Row  ss:AutoFitHeight="0" ss:Height="30.75" ss:StyleID="s21">
      <Cell ss:Index="2" ss:MergeAcross="1" ss:StyleID="s23">
        <Data ss:Type="String">
          IBERDROLA
        </Data>
      </Cell>
    </Row>
    <Row></Row>
    <xsl:apply-templates select="Tittle"/>
    <xsl:apply-templates select="HeaderAttributes"/>
    <Row></Row>
    <Row></Row>
  </xsl:template>

  <!--TEMPLATE HeaderAttributes-->
  <xsl:template match="HeaderAttributes">
    <xsl:apply-templates select="HeaderAttribute"/>
  </xsl:template>

  <!--TEMPLATE HeaderAttribute-->
  <xsl:template match="HeaderAttribute">
    <!--Escribe el contenido de el titulo de cabecera, aplicando el estilo correspondiente segun la posición que ocupe-->
    <xsl:choose>
      <!--Pos = 1-->
      <xsl:when test="position()=1">
        <Row  ss:AutoFitHeight="0">
          <xsl:apply-templates select="@name"/>
          <Cell ss:StyleID="s42" >
            <Data ss:Type="String">
              <xsl:value-of select="."/>
            </Data>
          </Cell>
          <Cell ss:Index="9" ss:StyleID="s43"/>
        </Row>
      </xsl:when>
      <!--Pos = ultimo-->
      <xsl:when test="position()=last()">
        <Row  ss:AutoFitHeight="0">
          <xsl:apply-templates select="@name"/>
          <Cell ss:StyleID="s55">
            <Data ss:Type="String">
              <xsl:value-of select="."/>
            </Data>
          </Cell>
        </Row>
      </xsl:when>
      <!--Pos = Los de en medio-->
      <xsl:otherwise>
        <Row  ss:AutoFitHeight="0">
          <xsl:apply-templates select="@name"/>
          <Cell ss:StyleID="s44">
            <Data ss:Type="String">
              <xsl:value-of select="."/>
            </Data>
          </Cell>

        </Row>

      </xsl:otherwise>
    </xsl:choose>

  </xsl:template>

  <!--TEMPLATE HeaderAttribute, Atributo NAME-->
  <xsl:template match="HeaderAttribute/@name">
    <!--escribe el titulo de la columna de atributo-->
    <Cell ss:Index="2" ss:MergeAcross="1" ss:StyleID="cuadrado">
      <Data ss:Type="String">
        <xsl:value-of select="."/>
      </Data>
    </Cell>
  </xsl:template>

  <!--TEMPLATE Title-->
  <xsl:template match="Tittle">
    <Row  ss:AutoFitHeight="0">
      <Cell  ss:Index="2" ss:MergeAcross="5" ss:MergeDown="3" ss:StyleID="m28024410">
        <Data ss:Type="String">
          <xsl:value-of select="."/>
        </Data>
      </Cell>
      <Cell ss:Index="10" ss:StyleID="s24"/>
      <Cell ss:StyleID="s24"/>
    </Row>
    <Row ss:AutoFitHeight="0">
      <Cell ss:Index="10" ss:StyleID="s24"/>
      <Cell ss:StyleID="s24"/>
    </Row>
    <Row ss:AutoFitHeight="0">
      <Cell ss:Index="10" ss:StyleID="s24"/>
      <Cell ss:StyleID="s24"/>
    </Row>
    <Row ss:AutoFitHeight="0">
      <Cell ss:Index="10" ss:StyleID="s24"/>
      <Cell ss:StyleID="s24"/>
    </Row>
    <Row ss:AutoFitHeight="0">
      <Cell ss:Index="6" ss:StyleID="s24"/>
      <Cell ss:StyleID="s24"/>
      <Cell ss:StyleID="s24"/>
      <Cell ss:StyleID="s24"/>
      <Cell ss:StyleID="s24"/>
      <Cell ss:StyleID="s24"/>
    </Row>

  </xsl:template>

  <!--TEMPLATE Data-->
  <xsl:template match="Data">
    <!--Llama a las templates que escriben los nombres de cabecera y los datos-->
    <xsl:apply-templates select="DataHeader"/>
    <xsl:apply-templates select="DataRows/Row"/>
  </xsl:template>

  <!--TEMPLATE DataHeader-->
  <xsl:template match="DataHeader">
    <Row ss:AutoFitHeight="0" ss:StyleID="s56">
      <xsl:apply-templates select="HeaderColumn"/>
    </Row>
  </xsl:template>

  <!--TEMPLATE HeaderColumn-->
  <xsl:template match="HeaderColumn">
    <!--En la primera columna tiene que empezar a escribir en la celda 2-->
    <xsl:choose>
      <xsl:when test="position()=1">
        <Cell ss:Index="2" ss:StyleID="s57">
          <Data ss:Type="String">
            <xsl:value-of select="@name"/>
          </Data>
          <NamedCell ss:Name="_FilterDatabase"/>
        </Cell>
      </xsl:when>
      <xsl:otherwise>
        <Cell  ss:StyleID="s57">
          <Data ss:Type="String">
            <xsl:value-of select="@name"/>
          </Data>
          <NamedCell ss:Name="_FilterDatabase"/>
        </Cell>

      </xsl:otherwise>
    </xsl:choose>
  </xsl:template>

  <!--TEMPLATE DataRows/Row-->
  <xsl:template match="DataRows/Row">
    <Row  ss:AutoFitHeight="0">
      <xsl:apply-templates select="RowColumn"/>
    </Row>
  </xsl:template>

  <!--TEMPLATE RowColumn-->
  <xsl:template match="RowColumn">
    <!--En la primera columna tiene que empezar a escribir en la celda 2-->
    <xsl:choose>
      <xsl:when test="position()=1">
        <Cell ss:Index="2" ss:StyleID="s58">
          <Data ss:Type="String">
            <xsl:value-of select="."/>
          </Data>
        </Cell>
      </xsl:when>
      <xsl:otherwise>
        <Cell ss:StyleID="s58">
          <Data ss:Type="String">
            <xsl:value-of select="."/>
          </Data>
        </Cell>
      </xsl:otherwise>
    </xsl:choose>
    
  </xsl:template>
  
</xsl:stylesheet>
