CREATE XML SCHEMA COLLECTION [dbo].[Markets]
    AS N'<xsd:schema xmlns:xsd="http://www.w3.org/2001/XMLSchema" elementFormDefault="qualified">
  <xsd:element name="Market">
    <xsd:complexType>
      <xsd:complexContent>
        <xsd:restriction base="xsd:anyType">
          <xsd:sequence>
            <xsd:element name="MarketType" minOccurs="0" maxOccurs="unbounded">
              <xsd:complexType>
                <xsd:complexContent>
                  <xsd:restriction base="xsd:anyType">
                    <xsd:sequence>
                      <xsd:element name="Group" minOccurs="0" maxOccurs="unbounded">
                        <xsd:complexType>
                          <xsd:complexContent>
                            <xsd:restriction base="xsd:anyType">
                              <xsd:sequence>
                                <xsd:element name="Entry" maxOccurs="unbounded">
                                  <xsd:complexType>
                                    <xsd:complexContent>
                                      <xsd:restriction base="xsd:anyType">
                                        <xsd:sequence />
                                        <xsd:attribute name="ID" type="xsd:unsignedInt" use="required" />
                                      </xsd:restriction>
                                    </xsd:complexContent>
                                  </xsd:complexType>
                                </xsd:element>
                              </xsd:sequence>
                              <xsd:attribute name="ID" type="xsd:string" use="required" />
                            </xsd:restriction>
                          </xsd:complexContent>
                        </xsd:complexType>
                      </xsd:element>
                      <xsd:element name="Entry" minOccurs="0" maxOccurs="unbounded">
                        <xsd:complexType>
                          <xsd:complexContent>
                            <xsd:restriction base="xsd:anyType">
                              <xsd:sequence />
                              <xsd:attribute name="ID" type="xsd:unsignedInt" use="required" />
                            </xsd:restriction>
                          </xsd:complexContent>
                        </xsd:complexType>
                      </xsd:element>
                    </xsd:sequence>
                    <xsd:attribute name="ID" type="xsd:string" use="required" />
                  </xsd:restriction>
                </xsd:complexContent>
              </xsd:complexType>
            </xsd:element>
            <xsd:element name="FilterType" minOccurs="0" maxOccurs="unbounded">
              <xsd:complexType>
                <xsd:complexContent>
                  <xsd:restriction base="xsd:anyType">
                    <xsd:sequence>
                      <xsd:element name="Group" minOccurs="0" maxOccurs="unbounded">
                        <xsd:complexType>
                          <xsd:complexContent>
                            <xsd:restriction base="xsd:anyType">
                              <xsd:sequence>
                                <xsd:element name="Entry" maxOccurs="unbounded">
                                  <xsd:complexType>
                                    <xsd:complexContent>
                                      <xsd:restriction base="xsd:anyType">
                                        <xsd:sequence />
                                        <xsd:attribute name="ID" type="xsd:string" use="required" />
                                      </xsd:restriction>
                                    </xsd:complexContent>
                                  </xsd:complexType>
                                </xsd:element>
                              </xsd:sequence>
                              <xsd:attribute name="ID" type="xsd:string" use="required" />
                            </xsd:restriction>
                          </xsd:complexContent>
                        </xsd:complexType>
                      </xsd:element>
                    </xsd:sequence>
                    <xsd:attribute name="ID" type="xsd:string" use="required" />
                  </xsd:restriction>
                </xsd:complexContent>
              </xsd:complexType>
            </xsd:element>
          </xsd:sequence>
        </xsd:restriction>
      </xsd:complexContent>
    </xsd:complexType>
  </xsd:element>
</xsd:schema>';

