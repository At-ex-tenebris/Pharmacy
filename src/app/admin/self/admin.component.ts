import {Component} from '@angular/core';
import { Link } from 'src/app/shared/models/link.model';
import { AdminHeaderComponent } from '../header/admin-header.component';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss'],
})
export class AdminComponent{
  headerLinks : Link[] = [
    new Link("Регионы", "admin/regions", ""),
    // здесь будут другие ссылки
  ]
  headerClassName : string = AdminHeaderComponent.MAIN_HEADER_CLASS_NAME;
  
}