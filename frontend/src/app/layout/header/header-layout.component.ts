import { Component } from "@angular/core";

@Component({
    selector: 'app-headerLayout',
    templateUrl: './header-layout.component.html',
    styleUrls: ['./header-layout.component.css']
})

export class HeaderLayoutComponent {
  
    title = 'Mini E-commerce project'
    
    menuItems: { title: string, route: string }[] = [
        {
            title: 'Product categories',
            route: '/productCategories'
        },
        {
            title: 'Products',
            route: '/products'
        },
    ];
}
