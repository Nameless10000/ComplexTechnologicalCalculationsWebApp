import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "../ui/table";
import { Card, CardContent, CardHeader, CardTitle } from "../ui/card";
import { ScrollArea } from "../ui/scroll-area";
import { Badge } from "../ui/badge";

export interface ComponentInfo {
  componentName: string;
  reportComponentOfShihta: number;
  reportFe: number;
  reportS: number;
  reportP: number;
  reportFeO: number;
  reportCaO: number;
  reportSiO2: number;
  reportAl2O3: number;
  reportMgO: number;
  reportMnO: number;
  reportTiO2: number;
  reportZn: number;
  reportPMPP: number;
  reportFe2O3: number;
  reportOxideSum: number;
  reportCaO_SiO2: number;
}

export interface AglomResponseData {
  components: ComponentInfo[];
}

interface SinterChargeResultsProps {
  results: AglomResponseData;
}

export function SinterChargeResults({ results }: SinterChargeResultsProps) {
  if (!results || !results.components || results.components.length === 0) {
    return (
      <div className="text-center p-8 text-muted-foreground">
        Нет данных для отображения
      </div>
    );
  }

  // Calculate totals or averages if needed, but for now just display the table
  // The last row usually contains totals in such engineering apps, 
  // but we will just render what is given.

  return (
    <div className="space-y-6">
      <Card>
        <CardHeader>
          <CardTitle>Результаты расчета агломерационной шихты</CardTitle>
        </CardHeader>
        <CardContent>
          <ScrollArea className="h-[600px] w-full rounded-md border">
            <div className="min-w-[1200px]"> 
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead className="w-[200px] sticky left-0 bg-background z-10">Компонент шихты</TableHead>
                    <TableHead>Расход (кг/100)</TableHead>
                    <TableHead>Fe</TableHead>
                    <TableHead>FeO</TableHead>
                    <TableHead>Fe₂O₃</TableHead>
                    <TableHead>SiO₂</TableHead>
                    <TableHead>CaO</TableHead>
                    <TableHead>Al₂O₃</TableHead>
                    <TableHead>MgO</TableHead>
                    <TableHead>MnO</TableHead>
                    <TableHead>TiO₂</TableHead>
                    <TableHead>S</TableHead>
                    <TableHead>P</TableHead>
                    <TableHead>Zn</TableHead>
                    <TableHead>ПМПП</TableHead>
                    <TableHead>Сумма оксидов</TableHead>
                    <TableHead className="text-right">Основность</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {results.components.map((comp, index) => (
                    <TableRow key={index} className={index === results.components.length - 1 ? "font-bold bg-muted/50" : ""}>
                      <TableCell className="font-medium sticky left-0 bg-background">{comp.componentName}</TableCell>
                      <TableCell>{comp.reportComponentOfShihta?.toFixed(2)}</TableCell>
                      <TableCell>{comp.reportFe?.toFixed(2)}</TableCell>
                      <TableCell>{comp.reportFeO?.toFixed(2)}</TableCell>
                      <TableCell>{comp.reportFe2O3?.toFixed(2)}</TableCell>
                      <TableCell>{comp.reportSiO2?.toFixed(2)}</TableCell>
                      <TableCell>{comp.reportCaO?.toFixed(2)}</TableCell>
                      <TableCell>{comp.reportAl2O3?.toFixed(2)}</TableCell>
                      <TableCell>{comp.reportMgO?.toFixed(2)}</TableCell>
                      <TableCell>{comp.reportMnO?.toFixed(2)}</TableCell>
                      <TableCell>{comp.reportTiO2?.toFixed(2)}</TableCell>
                      <TableCell>{comp.reportS?.toFixed(3)}</TableCell>
                      <TableCell>{comp.reportP?.toFixed(3)}</TableCell>
                      <TableCell>{comp.reportZn?.toFixed(3)}</TableCell>
                      <TableCell>{comp.reportPMPP?.toFixed(2)}</TableCell>
                      <TableCell>{comp.reportOxideSum?.toFixed(2)}</TableCell>
                      <TableCell className="text-right">
                         <Badge variant="outline">{comp.reportCaO_SiO2?.toFixed(2)}</Badge>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </div>
            <div className="p-4">
                <p className="text-sm text-muted-foreground">* Последняя строка обычно отображает итоговые показатели агломерата.</p>
            </div>
          </ScrollArea>
        </CardContent>
      </Card>
    </div>
  );
}
